using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoJsonSerialization
{
    static class ByteArrayConverter
    {
        public static string ToString(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", string.Empty);
        }

        public static byte[] ToByteArray(string hexString)
        {
            var byteList = new List<byte>();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                byteList.Add(Convert.ToByte(string.Empty + hexString[i] + hexString[i + 1], 16));
            }
            return byteList.ToArray();
        }
    }
}
