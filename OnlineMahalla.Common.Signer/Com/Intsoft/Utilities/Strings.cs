using System;
using System.Text;
using System.Globalization;

namespace Com.Intsoft.Utilities
{
	/// <summary> General string utilities.</summary>
	public sealed class Strings
	{
		private Strings()
		{
		}

		internal static bool IsOneOf(string s, params string[] candidates)
		{
			foreach (string candidate in candidates)
			{
				if (s == candidate)
					return true;
			}
			return false;
		}

		public static string FromByteArray(
			byte[] bs)
		{
			char[] cs = new char[bs.Length];
			for (int i = 0; i < cs.Length; ++i)
			{
				cs[i] = Convert.ToChar(bs[i]);
			}
			return new string(cs);
		}

        public static byte[] ToByteArray(
            char[] cs)
        {
            byte[] bs = new byte[cs.Length];
            for (int i = 0; i < bs.Length; ++i)
            {
                bs[i] = Convert.ToByte(cs[i]);
            }
            return bs;
        }

        public static byte[] ToByteArray(
			string s)
		{
			byte[] bs = new byte[s.Length];
			for (int i = 0; i < bs.Length; ++i)
			{
				bs[i] = Convert.ToByte(s[i]);
			}
			return bs;
		}

        public static string FromAsciiByteArray(
            byte[] bytes)
        {
#if SILVERLIGHT
            // TODO Check for non-ASCII bytes in input?
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
#else
            return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
#endif
        }

        public static byte[] ToAsciiByteArray(
            char[] cs)
        {
#if SILVERLIGHT
            // TODO Check for non-ASCII characters in input?
            return Encoding.UTF8.GetBytes(cs);
#else
            return Encoding.ASCII.GetBytes(cs);
#endif
        }

        public static byte[] ToAsciiByteArray(
            string s)
        {
#if SILVERLIGHT
            // TODO Check for non-ASCII characters in input?
            return Encoding.UTF8.GetBytes(s);
#else
            return Encoding.ASCII.GetBytes(s);
#endif
        }

        public static string FromUtf8ByteArray(
			byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

        public static byte[] ToUtf8ByteArray(
            char[] cs)
        {
            return Encoding.UTF8.GetBytes(cs);
        }

		public static byte[] ToUtf8ByteArray(
			string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}

        public DateTime ToDateTime(string tm)
        {
            //yyyy.oo.kk ss:mm:ss
            //0123456789012345678
            return new DateTime(
                Int16.Parse(tm.Substring(0, 4)),
                Int16.Parse(tm.Substring(5, 2)),
                Int16.Parse(tm.Substring(8, 2)),
                Int16.Parse(tm.Substring(11, 2)),
                Int16.Parse(tm.Substring(14, 2)),
                Int16.Parse(tm.Substring(17, 2)));
        }
        public DateTime ToUTCDateTime(string tm)
        {
            return DateTime.ParseExact(tm, @"yyyy.MM.dd hh:mm:ss", DateTimeFormatInfo.InvariantInfo).ToUniversalTime();
        }
    }
}
