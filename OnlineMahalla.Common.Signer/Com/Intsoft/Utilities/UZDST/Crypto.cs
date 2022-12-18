using System;
using Com.Intsoft.Crypto.Digests;
using Com.Intsoft.Crypto.Parameters;
using Com.Intsoft.Crypto.Signers;
using Com.Intsoft.Math;

namespace Com.Intsoft.Utilities.UZDST
{
    public class Crypto
    {
        public static byte[] CalcDigest(byte[] input)
        {
            return CalcDigest(input, 0, input.Length);
        }

        public static byte[] CalcDigest(byte[] input, int inOff, int length)
        {
            UZDST1106Digest digest = new UZDST1106Digest();
            byte[] outBytes = new byte[digest.GetDigestSize()];
            digest.Reset();
            digest.BlockUpdate(input, inOff, length);
            digest.DoFinal(outBytes, 0);
            return outBytes;
        }

        public static byte[] SignDigest(byte[] digestBytes, byte[] privateKeyBytes, byte[] publicKeyBytes)
        {
            UZDST1092Signer signer = new UZDST1092Signer();
            UZDST1092Parameters parameters = Keys.GetUZDST1092Parameters(publicKeyBytes);
            signer.Init(true, Keys.GetUZDST1092PrivateKeyParams(parameters, privateKeyBytes));
            BigInteger[] sign = signer.GenerateSignature(digestBytes);
            byte[] r = sign[0].ToByteArray();
            byte[] s = sign[1].ToByteArray();
            byte[] outBytes = new byte[r.Length + s.Length];
            Array.Copy(r, 0, outBytes, 0, r.Length);
            Array.Copy(s, 0, outBytes, r.Length, s.Length);
            return outBytes;
        }
    }
}
