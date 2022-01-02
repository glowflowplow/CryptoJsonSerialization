using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoJsonSerialization
{
    internal static class Utils
    {
        public static bool IsNumberType(Type type)
        {
            var numTypes = new Type[] {
                        typeof(byte),
                        typeof(sbyte),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(float),
                        typeof(double),
                        typeof(decimal)
                    };
            return numTypes.Contains(type);
        }
    }
}
