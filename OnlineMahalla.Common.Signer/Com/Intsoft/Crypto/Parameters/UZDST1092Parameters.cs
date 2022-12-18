using System;

using Com.Intsoft.Crypto;
using Com.Intsoft.Math;

namespace Com.Intsoft.Crypto.Parameters
{
    public class UZDST1092Parameters : ICipherParameters
    {
        private readonly BigInteger p, q, R;
        public UZDST1092Parameters(
            BigInteger p,
            BigInteger q,
            BigInteger R)
        {
            this.p = p;
            this.q = q;
            this.R = R;
        }
        public BigInteger GetP()
        {
            return p;
        }

        public BigInteger GetQ()
        {
            return q;
        }

        public BigInteger GetR()
        {
            return R;
        }

        public override bool Equals(
            Object obj)
        {
            if (obj == this)
                return true;

            UZDST1092Parameters other = obj as UZDST1092Parameters;

            if (other == null)
                return false;

            return Equals(other);
        }

        protected bool Equals(
            UZDST1092Parameters other)
        {
            return p.Equals(other.p)
                && q.Equals(other.q)
                && R.Equals(other.R);
        }
        public override int GetHashCode()
        {
           return p.GetHashCode() ^ q.GetHashCode() ^ R.GetHashCode();
        }
    }
}
