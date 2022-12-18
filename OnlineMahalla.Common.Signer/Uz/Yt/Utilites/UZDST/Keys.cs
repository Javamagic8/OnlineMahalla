using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Intsoft.Crypto.Parameters;
using Com.Intsoft.Math;

namespace isigner.Uz.Yt.Utilites.UZDST
{
    public class Keys
    {
        // common params
        private const int PBK_P_SIZE = 128;
        private const int PBK_Q_SIZE = 32;
        private const int PBK_R_SIZE = 32;
        private const int PBK_R1_SIZE = 32;

        // publik key params
        private const int DH_POW_SIZE = PBK_P_SIZE;
        private const int PBK_Y_SIZE = PBK_P_SIZE;
        private const int PBK_Z_SIZE = PBK_P_SIZE;

        private const int DH_POW_OFFSET = 0;
        private const int PBK_Y_OFFSET = DH_POW_OFFSET + DH_POW_SIZE;
        private const int PBK_Z_OFFSET = PBK_Y_OFFSET + PBK_Y_SIZE;

        // private key params
        private const int DH_KEY_SIZE = 64;
        private const int PRK_X_SIZE = 32;
        private const int PRK_U_SIZE = 32;
        private const int PRK_G_SIZE = 128;

        private const int DH_KEY_OFFSET = 0;
        private const int PRK_X_OFFSET = DH_KEY_OFFSET + DH_KEY_SIZE;
        private const int PRK_U_OFFSET = PRK_X_OFFSET + PRK_X_SIZE;
        private const int PRK_G_OFFSET = PRK_U_OFFSET + PRK_U_SIZE;

        public static UZDST1092Parameters SICNT_UZDST1092Parameters = new UZDST1092Parameters(
            new BigInteger("DB1AF335208E5B2CB9F6A3A1B1D0E1629057FF7ECFAC1AD2295EE37BCA690F6D19FA63C6AE8E873320BF594B52A54D4FFC4FCFAF96E53CB8943FB0BD493510597BD6CB3C5DB7E1B50B7743C6F3DA0D28CA319E04181B3AE9F03F1478AC63A6B671F95CE98DF4B31247A54FF0AD76B0DABA309C72ED25160CE297305E7D7CBC8D", 16),
            new BigInteger("7C333B9D0106595CCBE2B629340459371DCFABDC2B3245CFFF45DD899CC781A1", 16),
            new BigInteger("ECCECC3373CA962A300232466EFD2B08CA426EF593D0A9E829A7A6051BD4F681", 16)
        );

        public static UZDST1092PublicKeyParameters GetUZDST1092PublicKeyParams(byte[] publicKeyBytes)
        {
            byte[] dh_pow = new byte[DH_POW_SIZE];
            byte[] pbk_y = new byte[PBK_Y_SIZE];
            byte[] pbk_z = new byte[PBK_Z_SIZE];
            Array.Copy(publicKeyBytes, DH_POW_OFFSET, dh_pow, 0, DH_POW_SIZE);
            Array.Copy(publicKeyBytes, PBK_Y_OFFSET, pbk_y, 0, PBK_Y_SIZE);
            Array.Copy(publicKeyBytes, PBK_Z_OFFSET, pbk_z, 0, PBK_Z_SIZE);
            return new UZDST1092PublicKeyParameters(
                new BigInteger(1, pbk_y),
                new BigInteger(1, pbk_z),
                SICNT_UZDST1092Parameters
            );
        }

        public static UZDST1092PublicKeyParameters GetUZDST1092PublicKeyParams(UZDST1092Parameters parameters, byte[] publicKeyBytes)
        {
            byte[] dh_pow = new byte[DH_POW_SIZE];
            byte[] pbk_y = new byte[PBK_Y_SIZE];
            byte[] pbk_z = new byte[PBK_Z_SIZE];
            Array.Copy(publicKeyBytes, DH_POW_OFFSET, dh_pow, 0, DH_POW_SIZE);
            Array.Copy(publicKeyBytes, PBK_Y_OFFSET, pbk_y, 0, PBK_Y_SIZE);
            Array.Copy(publicKeyBytes, PBK_Z_OFFSET, pbk_z, 0, PBK_Z_SIZE);
            return new UZDST1092PublicKeyParameters(
                new BigInteger(1, pbk_y),
                new BigInteger(1, pbk_z),
                parameters
            );
        }

        public static UZDST1092PrivateKeyParameters GetUZDST1092PrivateKeyParams(byte[] publicKeyBytes, byte[] privateKeyBytes)
        {
            byte[] dh_key = new byte[DH_KEY_SIZE];
            byte[] prk_x = new byte[PRK_X_SIZE];
            byte[] prk_u = new byte[PRK_U_SIZE];
            byte[] prk_g = new byte[PRK_G_SIZE];
            Array.Copy(privateKeyBytes, DH_KEY_OFFSET, dh_key, 0, DH_KEY_SIZE);
            Array.Copy(privateKeyBytes, PRK_X_OFFSET, prk_x, 0, PRK_X_SIZE);
            Array.Copy(privateKeyBytes, PRK_U_OFFSET, prk_u, 0, PRK_U_SIZE);
            Array.Copy(privateKeyBytes, PRK_G_OFFSET, prk_g, 0, PRK_G_SIZE);
            return new UZDST1092PrivateKeyParameters(
                new BigInteger(1, prk_x),
                new BigInteger(1, prk_u),
                new BigInteger(1, prk_g),
                SICNT_UZDST1092Parameters
            );
        }

        public static UZDST1092PrivateKeyParameters GetUZDST1092PrivateKeyParams(UZDST1092Parameters parameters, byte[] privateKeyBytes)
        {
            byte[] dh_key = new byte[DH_KEY_SIZE];
            byte[] prk_x = new byte[PRK_X_SIZE];
            byte[] prk_u = new byte[PRK_U_SIZE];
            byte[] prk_g = new byte[PRK_G_SIZE];
            Array.Copy(privateKeyBytes, DH_KEY_OFFSET, dh_key, 0, DH_KEY_SIZE);
            Array.Copy(privateKeyBytes, PRK_X_OFFSET, prk_x, 0, PRK_X_SIZE);
            Array.Copy(privateKeyBytes, PRK_U_OFFSET, prk_u, 0, PRK_U_SIZE);
            Array.Copy(privateKeyBytes, PRK_G_OFFSET, prk_g, 0, PRK_G_SIZE);
            return new UZDST1092PrivateKeyParameters(
                new BigInteger(1, prk_x),
                new BigInteger(1, prk_u),
                new BigInteger(1, prk_g),
                parameters
            );
        }

        public static BigInteger[] GetUZDST1092Signature(byte[] signatureBytes)
        {
            byte[] r = new byte[PBK_P_SIZE];
            byte[] s = new byte[PBK_Q_SIZE];
            Array.Copy(signatureBytes, 0, r, 0, r.Length);
            Array.Copy(signatureBytes, r.Length, s, 0, s.Length);

            return new BigInteger[] { new BigInteger(1, r), new BigInteger(1, s) };
        }
        public static int GetPSize()
        {
            return PBK_P_SIZE;
        }
        public static int GetQSize()
        {
            return PBK_P_SIZE;
        }
    }
}
