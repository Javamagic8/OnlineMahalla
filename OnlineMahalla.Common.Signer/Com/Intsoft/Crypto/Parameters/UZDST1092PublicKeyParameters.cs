using System;
using Com.Intsoft.Math;

namespace Com.Intsoft.Crypto.Parameters
{

    public class UZDST1092PublicKeyParameters : UZDST1092KeyParameters
    {
        private BigInteger      y, z;
        public UZDST1092PublicKeyParameters(
            BigInteger      y,
            BigInteger      z,
            UZDST1092Parameters   parameters)
            : base(false, parameters)
        {
            this.y = y;
            this.z = z;
        }   

        public BigInteger GetY()
        {
            return y;
        }
        public BigInteger GetZ()
        {
            return z;
        } 
    }
}
