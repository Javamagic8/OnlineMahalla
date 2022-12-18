using System;
using System.Collections;
using Com.Intsoft.Crypto.Parameters;
using Com.Intsoft.Math;

namespace Com.Intsoft.Utilities.UZDST
{
    public sealed class Keys
    {
        private const int DH_N_SIZE = 128;
        private const int DH_G_SIZE = 32;
        private const int DH_A_SIZE = 128;
        private const int PBK_P_SIZE = 128;
        private const int PBK_Q_SIZE = 32;
        private const int PBK_R_SIZE = 32;
        private const int PBK_R1_SIZE = 32;
        private const int PBK_Y_SIZE = PBK_P_SIZE;
        private const int PBK_Z_SIZE = PBK_P_SIZE;
        
        private const int DH_N_OFFSET = 0;
        private const int DH_G_OFFSET = DH_N_OFFSET + DH_N_SIZE;
        private const int PBK_P_OFFSET = DH_G_OFFSET + DH_G_SIZE;
        private const int DH_A_OFFSET = PBK_P_OFFSET + PBK_P_SIZE;
        private const int PBK_Q_OFFSET = DH_A_OFFSET + DH_A_SIZE;
        private const int PBK_R_OFFSET = PBK_Q_OFFSET + PBK_Q_SIZE;
        private const int PBK_R1_OFFSET = PBK_R_OFFSET + PBK_R_SIZE;
        private const int PBK_Y_OFFSET = PBK_R1_OFFSET + PBK_R1_SIZE;
        private const int PBK_Z_OFFSET = PBK_Y_OFFSET + PBK_Y_SIZE;

        private const int DH_B_SIZE = 64;
        private const int PRK_X_SIZE = 32;
        private const int PRK_U_SIZE = 32;
        private const int PRK_G_SIZE = 128;

        private const int DH_B_OFFSET = 0;
        private const int PRK_X_OFFSET = DH_B_OFFSET + DH_B_SIZE;
        private const int PRK_U_OFFSET = PRK_X_OFFSET + PRK_X_SIZE;
        private const int PRK_G_OFFSET = PRK_U_OFFSET + PRK_U_SIZE;

        public static UZDST1092Parameters GetUZDST1092Parameters(byte[] publicKeyBytes) {
            byte[] pbk_p = new byte[PBK_P_SIZE];
            byte[] pbk_q = new byte[PBK_Q_SIZE];
            byte[] pbk_R = new byte[PBK_R_SIZE];
            Array.Copy(publicKeyBytes, PBK_P_OFFSET, pbk_p, 0, PBK_P_SIZE);
            Array.Copy(publicKeyBytes, PBK_Q_OFFSET, pbk_q, 0, PBK_Q_SIZE);
            Array.Copy(publicKeyBytes, PBK_R_OFFSET, pbk_R, 0, PBK_R_SIZE);
            return new UZDST1092Parameters(
                new BigInteger(1, pbk_p),
                new BigInteger(1, pbk_q),
                new BigInteger(1, pbk_R)
            );
        }

        public static UZDST1092PublicKeyParameters GetUZDST1092PublicKeyParams(byte[] publicKeyBytes)
        {
            byte[] pbk_y = new byte[PBK_Y_SIZE];
            byte[] pbk_z = new byte[PBK_Z_SIZE];
            Array.Copy(publicKeyBytes, PBK_Y_OFFSET, pbk_y, 0, PBK_Y_SIZE);
            Array.Copy(publicKeyBytes, PBK_Z_OFFSET, pbk_z, 0, PBK_Z_SIZE);
            return new UZDST1092PublicKeyParameters(
                new BigInteger(1, pbk_y),
                new BigInteger(1, pbk_z),
                GetUZDST1092Parameters(publicKeyBytes)
            );
        }

        public static UZDST1092PublicKeyParameters GetUZDST1092PublicKeyParams(UZDST1092Parameters parameters, byte[] publicKeyBytes)
        {
            byte[] pbk_y = new byte[PBK_Y_SIZE];
            byte[] pbk_z = new byte[PBK_Z_SIZE];
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
            byte[] prk_x = new byte[PRK_X_SIZE];
            byte[] prk_u = new byte[PRK_U_SIZE];
            byte[] prk_g = new byte[PRK_G_SIZE];
            Array.Copy(privateKeyBytes, PRK_X_OFFSET, prk_x, 0, PRK_X_SIZE);
            Array.Copy(privateKeyBytes, PRK_U_OFFSET, prk_u, 0, PRK_U_SIZE);
            Array.Copy(privateKeyBytes, PRK_G_OFFSET, prk_g, 0, PRK_G_SIZE);
            return new UZDST1092PrivateKeyParameters(
                new BigInteger(1, prk_x),
                new BigInteger(1, prk_u),
                new BigInteger(1, prk_g),
                GetUZDST1092Parameters(publicKeyBytes)
            );
        }

        public static UZDST1092PrivateKeyParameters GetUZDST1092PrivateKeyParams(UZDST1092Parameters parameters, byte[] privateKeyBytes)
        {
            byte[] prk_x = new byte[PRK_X_SIZE];
            byte[] prk_u = new byte[PRK_U_SIZE];
            byte[] prk_g = new byte[PRK_G_SIZE];
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
