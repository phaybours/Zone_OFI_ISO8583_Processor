using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zone_OFI_ISO8583_Processor.Utilities
{
    public static class XORHelper
    {
        public static string XorHexStrings(string hex1, string hex2)
        {
            // Convert the hexadecimal strings to byte arrays
            byte[] bytes1 = HexStringToByteArray(hex1);
            byte[] bytes2 = HexStringToByteArray(hex2);

            // Ensure the byte arrays are of the same length
            int length = Math.Min(bytes1.Length, bytes2.Length);
            byte[] result = new byte[length];

            // Perform XOR on each byte
            for (int i = 0; i < length; i++)
            {
                result[i] = (byte)(bytes1[i] ^ bytes2[i]);
            }

            // Convert the result back to a hexadecimal string
            return ByteArrayToHexString(result);
        }

        static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        static string ByteArrayToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
