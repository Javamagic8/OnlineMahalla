using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Utility
{
    public static class StringUtility
    {
        public static string GetFIO(string input)
        {
            if (input == "" || input == null)
                return "";
            string res = "";
            try
            {
                input = input.Trim();
                string[] fio = input.Split(new string[] { " " }, StringSplitOptions.None);
                if (fio.Length >= 3)
                {
                    res = fio[1].Substring(0, (fio[1].ToLower().StartsWith("sh") || fio[1].ToLower().StartsWith("ch") ? 2 : 1)) + "." + fio[2].Substring(0, (fio[2].ToLower().StartsWith("sh") || fio[2].ToLower().StartsWith("ch") ? 2 : 1)) + "." + fio[0];
                }
                else
                {
                    res = fio[1].Substring(0, (fio[1].ToLower().StartsWith("sh") || fio[1].ToLower().StartsWith("ch") ? 2 : 1)) + "." + fio[0];
                }
            }
            catch
            {

            }
            if (res == "")
                return input;
            else
                return res;
        }
        public static string RemoveUzbekChars(string input)
        {
            if (input == null)
                return "";

            input = input.Trim();

            string[] repl = { "Қ", "қ", "Ғ", "ғ", "Ҳ", "ҳ", "Ў", "ў", "\r", "\t", "\n", "\v", "\f" };
            string[] replWith = { "К", "к", "Г", "г", "Х", "х", "У", "у", "", "", " ", "", "" };

            for (int i = 0; i < repl.Count(); i++)
            {
                input = input.Replace(repl[i], replWith[i]);
            }

            return input;
        }
        public static string SumToTreasury(decimal? Value)
        {
            if (Value == null)
                return "0";
            return (Value * 100).ToString().Replace(",00", "").Replace(".00", "");
        }
        public static string SumToTreasury2(decimal? Value)
        {
            if (Value == null)
                return "0";
            return (Value).ToString().Replace(",", "").Replace(".", "");
        }
#if !SILVERLIGHT
        public class Crc32 : HashAlgorithm
        {
            public const UInt32 DefaultPolynomial = 0xedb88320;
            public const UInt32 DefaultSeed = 0xffffffff;

            private UInt32 hash;
            private UInt32 seed;
            private UInt32[] table;
            private UInt32[] defaultTable;

            public Crc32()
            {
                table = InitializeTable(DefaultPolynomial);
                seed = DefaultSeed;
                Initialize();
            }

            public Crc32(UInt32 polynomial, UInt32 seed)
            {
                table = InitializeTable(polynomial);
                this.seed = seed;
                Initialize();
            }

            public override void Initialize()
            {
                hash = seed;
            }

            protected override void HashCore(byte[] buffer, int start, int length)
            {
                hash = CalculateHash(table, hash, buffer, start, length);
            }

            protected override byte[] HashFinal()
            {
                byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
                this.HashValue = hashBuffer;
                return hashBuffer;
            }

            public override int HashSize
            {
                get { return 32; }
            }

            public UInt32 Compute(byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
            }

            public UInt32 Compute(UInt32 seed, byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
            }

            public UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
            }

            private UInt32[] InitializeTable(UInt32 polynomial)
            {
                if (polynomial == DefaultPolynomial && defaultTable != null)
                    return defaultTable;

                UInt32[] createTable = new UInt32[256];
                for (int i = 0; i < 256; i++)
                {
                    UInt32 entry = (UInt32)i;
                    for (int j = 0; j < 8; j++)
                        if ((entry & 1) == 1)
                            entry = (entry >> 1) ^ polynomial;
                        else
                            entry = entry >> 1;
                    createTable[i] = entry;
                }

                if (polynomial == DefaultPolynomial)
                    defaultTable = createTable;

                return createTable;
            }

            private UInt32 CalculateHash(UInt32[] table, UInt32 seed, byte[] buffer, int start, int size)
            {
                UInt32 crc = seed;
                for (int i = start; i < size; i++)
                    unchecked
                    {
                        crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                    }
                return crc;
            }

            private byte[] UInt32ToBigEndianBytes(UInt32 x)
            {
                return new byte[] {
            (byte)((x >> 24) & 0xff),
            (byte)((x >> 16) & 0xff),
            (byte)((x >> 8) & 0xff),
            (byte)(x & 0xff)
        };
            }
        }

#endif

    }
}
