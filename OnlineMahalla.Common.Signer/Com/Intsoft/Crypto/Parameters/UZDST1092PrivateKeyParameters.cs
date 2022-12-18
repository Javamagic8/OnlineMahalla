using System;
using Com.Intsoft.Math;

namespace Com.Intsoft.Crypto.Parameters
{
    public class UZDST1092PrivateKeyParameters : UZDST1092KeyParameters
    {
        private BigInteger      x, u, g;

        public UZDST1092PrivateKeyParameters(
            BigInteger      x,
            BigInteger      u,
            BigInteger      g,
            UZDST1092Parameters   parameters)
            : base(true, parameters)
        {
            this.x = x;
            this.u = u;
            this.g = g;
        }
        public BigInteger GetX()
        {
            return x;
        }
        public BigInteger GetU()
        {
            return u;
        }
        public BigInteger GetG()
        {
            return g;
        }
    }
}
