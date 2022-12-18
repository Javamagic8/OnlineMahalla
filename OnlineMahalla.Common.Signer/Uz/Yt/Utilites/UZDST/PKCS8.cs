using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isigner.Uz.Yt.Utilites.UZDST
{
    public class PKCS8
    {
        public static byte[] GetPrivateKey(byte[] pkcs8)
        {
            byte[] pbk = new byte[256];
            int ofs = 37;
            if (pkcs8.Length == 301) ofs = 41;
            Array.Copy(pkcs8, ofs + 4, pbk, 0, 256);
            return pbk;
        }
    }
}
