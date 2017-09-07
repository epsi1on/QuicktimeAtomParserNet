using System;
using System.IO;
using System.Linq;
using System.Numerics;

namespace QicktimeAtomParserNet.Lib
{
    static class util
    {

        public static long ToLong(this byte[] bytes)
        {
            BigInteger atomSizeBI = new BigInteger(bytes.Reverse().ToArray());

            return (long)atomSizeBI;
        }

        public static byte[] ReadBytes(this Stream str, int count)
        {
            var buf = new byte[count];

            str.Read(buf, 0, buf.Length);

            return buf;
        }

        public static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string SizeSuffix(Int64 value, int decimalPlaces = 0)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
    }
}
