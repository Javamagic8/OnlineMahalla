using System;

using Com.Intsoft.Crypto.Parameters;
using Com.Intsoft.Math;
using Com.Intsoft.Security;
using Com.Intsoft.Crypto.Digests;

namespace Com.Intsoft.Crypto.Signers
{
	public class UZDST1092Signer
		: IDsa
	{
		private UZDST1092KeyParameters key;

		public string AlgorithmName
		{
			get { return "UZDST1092"; }
		}

		public void Init(
			bool				forSigning,
			ICipherParameters	parameters)
		{
			if (forSigning)
			{
				if (!(parameters is UZDST1092PrivateKeyParameters))
					throw new InvalidKeyException("UZDST1092 private key required for signing");

				this.key = (UZDST1092PrivateKeyParameters) parameters;
			}
			else
			{
				if (!(parameters is UZDST1092PublicKeyParameters))
					throw new InvalidKeyException("UZDST1092 public key required for signing");

				this.key = (UZDST1092PublicKeyParameters) parameters;
			}
		}

		/**
		 * generate a signature for the given message using the key we were
		 * initialised with. For conventional Gost3410 the message should be a Gost3411
		 * hash of the message of interest.
		 *
		 * @param message the message that will be verified later.
		 */
		public BigInteger[] GenerateSignature(
			byte[] message)
		{
			UZDST1092PrivateKeyParameters privKey = (UZDST1092PrivateKeyParameters)key;
			BigInteger P = key.GetParameters().GetP();
			BigInteger Q = key.GetParameters().GetQ();
			BigInteger R = key.GetParameters().GetR();

			BigInteger r = null;
			BigInteger s = null;

			// step 1: m = H(M)
			BigInteger m = new BigInteger(1, message);
			BigInteger c = privKey.GetX();

			// step 2: k = H(m + ( 1 + m В· R ) В· c) i.e. k = H(m В® c)
			BigInteger tmp = Dia.Multiple(m, c, R);
			UZDST1106Digest digest = new UZDST1106Digest();
			byte[] tmp_buf = tmp.ToByteArray();
			digest.BlockUpdate(tmp_buf, (tmp_buf[0] == 0 ? 1 : 0), (tmp_buf[0] == 0 ? tmp_buf.Length - 1 : tmp_buf.Length));
			byte[] k_buf = new byte[digest.GetDigestSize()];
			digest.DoFinal(k_buf, 0);

			BigInteger k = new BigInteger(1, k_buf);

			// step 3: T = g\-k 
			BigInteger T = null;
			BigInteger s1 = null;
			bool r_mod_q_is_zero = false;
			bool s1_mod_q_is_zero = false;
			do
			{
				do
				{
					tmp = Dia.Pow(privKey.GetG(), k, R, P);
					T = Dia.ModInverse(tmp, R, P);

					// step 4: r =  (m + (1 + m В· R ) В· T) % p  i.e. r = (m В® T) % p
					r = Dia.MultipleMod(m, T, R, P);
					tmp = r.Mod(Q);

					if (tmp.LongValue == 0)
					{
						k.Add(BigInteger.One).Mod(P);
						r_mod_q_is_zero = true;
					}
				} while (r_mod_q_is_zero);

				// step 5: s1 = (k - r В· x) % q
				s1 = k.Subtract(r.Multiply(privKey.GetX())).Mod(Q);

				if (s1.LongValue == 0)
				{
					k.Add(BigInteger.One).Mod(P);
					s1_mod_q_is_zero = true;
				}
			} while (s1_mod_q_is_zero);

			// step 6: s = (s1 В· u^вЂ“1) % q
			s = privKey.GetU().ModInverse(Q).Multiply(s1).Mod(Q); 
			return new BigInteger[] {r, s};
		}
		public bool VerifySignature(
			byte[] message,
			BigInteger r,
			BigInteger s)
		{
			BigInteger P = key.GetParameters().GetP();
			BigInteger Q = key.GetParameters().GetQ();
			BigInteger R = key.GetParameters().GetR();
			UZDST1092PublicKeyParameters pubKey = (UZDST1092PublicKeyParameters)key;

			/* step 2: if Len(s)>Len(q) OR Len(r)>Len(p) then sign is unauthentic*/
			if (s.BitLength > Q.BitLength || r.BitLength > P.BitLength)
			{
				return false;
			}

			/* step 1: m = H(M)*/
			BigInteger m = new BigInteger(1, message);

			/* step 3: z0 = (z\s)%p */
			BigInteger z0 = Dia.Pow(pubKey.GetZ(), s, R, P);

			/* step 4: rвЂ™ =  r%q */
			BigInteger r_ = r.Mod(Q);

			/* step 5: y2 = (y\rвЂ™)%p */
			BigInteger y2 = Dia.Pow(pubKey.GetY(), r_, R, P);

			/* step 6: z1 = (z0 + (1+ z0 В· R) В· y2)%p i.e. z1 = (z0 В® y2) % p */
			BigInteger z1 = Dia.MultipleMod(z0, y2, R, P);

			/* step 7:  y3 = (z1 + (1 + z1 В· R) В· r)%p i.e. y3 = (z1 В® r) % p */
			BigInteger y3 = Dia.MultipleMod(z1, r, R, P);

			return (y3.Equals(m));
		}
	}
}
