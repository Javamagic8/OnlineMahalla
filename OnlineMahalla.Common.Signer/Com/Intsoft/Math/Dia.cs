using System;

namespace Com.Intsoft.Math
{
    public class Dia
    {
        /*
         * Calulates X ® Y = X + Y + X · R · Y.
         */
        public static BigInteger Multiple(BigInteger x, BigInteger y, BigInteger R)
        {
            return x.Multiply(y).Multiply(R).Add(x).Add(y);
        }
        /*
         * Calulates (X ® Y)%p = (X + Y + X · Y · R) % p.
         */
        public static BigInteger MultipleMod(BigInteger x, BigInteger y, BigInteger R, BigInteger p)
        {
            return Multiple(x, y, R).Mod(p);
        }

        /*
         * Calulates X\Y the dia power
         * Example: (X\37) % p = (X\(32 + 4 + 1)) % p = (((((X\2)\2)\2)\2)\2 ® (X\2)\2 ® X) % p,
         *          where (X\2) % p = (X · (2 + X · R)) % p;
         */
        public static BigInteger Pow(
            BigInteger x, 
            BigInteger y, 
            BigInteger R, 
            BigInteger p)
        {
            BigInteger res, tmp;
            long ylong = y.LongValue;

            if (ylong == 0)
                return BigInteger.One;
            else if (ylong == 1)
                return x;
            else
            {
                int bits = 0;
                int shift = 0;

                bits = y.BitLength;
                tmp = x;
                if (y.TestBit(0))
                    res = x;
                else
                    res = BigInteger.Zero;
                while (++shift < bits)
                {
                    tmp = MultipleMod(tmp, tmp, R, p);
                    if (y.TestBit(shift))
                        res = MultipleMod(res, tmp, R, p);
                }
                return res;
            }
        }
        /*
         * Calulates X\-1 with R mod p the dia mod inverse
         * X\-1 = (-X · ( 1 + X · R )^(-1)) (mod p).
         */
        public static BigInteger ModInverse(BigInteger x, BigInteger R, BigInteger p)
        {
            return x.Multiply(R).Add(BigInteger.One).Mod(p).ModInverse(p).Multiply(p.Subtract(x)).Mod(p);
        }



        /*
        ** dia kvadratga oshirhish a^\2 (mod modulo)
        ** Kirish: const dword a, const byte R, const dword modulo
        ** Chiqish: dword
        */
        private static int SqrInt(int a, int R, int modulo){
	        return (a * R + 2) * a % modulo;
        }

        /*
        ** dia ko`paytirish a пїЅ b (mod modulo)
        ** Kirish: const dword a, const dword b, const byte R, const dword modulo
        ** Chiqish: dword
        */
        private static int MulInt(int a, int b, int R, int modulo){
	        return (a * b * R + a + b) % modulo;
        }

        private static bool IsBitSet(int x, int y){
            return (((x) & (1 << (y))) > 0);
        }
        private static int NumBits(int x){
	        int n = 1;
	        if (x == 0) return 0;
                x >>= 1;
                while(x != 0){
                    x >>= 1;
                    n++;
                }
	        return n;
        }
        /*
        ** dia daraja base^\pow (mod modulo)
        ** Masalan: ((base^\37) (mod modulo) = (base^\(32 + 4 + 1)) (mod modulo) = (((((base^\2)^\2)^\2)^\2)^\2 пїЅ (base^\2)^\2 пїЅ base) (mod modulo),
        ** bu yerda: (base^\2) (mod modulo) = (base * (2 + base * R)) (mod modulo);
        ** Kirish: const dword base, const byte pow, const byte R, const dword modulo
        ** Chiqish: dword
        */
        public static int PowInt(int b, int pow, int R, int modulo)
        {
            int bits;
            int i;
            int t, res;
            if (pow == 0) return 1;
            if (pow == 1) return b;
            bits = NumBits(pow);
            t = b;
            res = (IsBitSet(pow, 0) ? t : 0);
            for (i = 1; i < bits; i++)
            {
                t = SqrInt(t, R, modulo);
                if (IsBitSet(pow, i))
                    res = MulInt(res, t, R, modulo);
            }
            return res;
        }

    }
}
