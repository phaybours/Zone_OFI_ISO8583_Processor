using System;
using System.Linq;

public static class Iso8583PinBlockGenerator
{
    public static string GenerateIso0PinBlock(string pin, string pan)
    {
        // Step 1: Format the PIN block
        string pinBlock = FormatPinBlock(pin);

        // Step 2: Format the PAN block
        string panBlock = FormatPanBlock(pan);

        // Step 3: XOR the PIN block and PAN block to get the Clear PIN block
        string clearPinBlock = XorHexStrings(pinBlock, panBlock);

        return clearPinBlock;
    }

    private static string FormatPinBlock(string pin)
    {
        // Length of the PIN
        int pinLength = pin.Length;

        // '0' indicates using ISO-0 format
        // Padding with 'F' to make it 16 characters long
        string pinBlock = $"0{pinLength}{pin}".PadRight(16, 'F');

        return pinBlock;
    }

    private static string FormatPanBlock(string pan)
    {
        // Get the 12 rightmost digits of the PAN (excluding the check digit)
        string pan12Digits = pan.Substring(pan.Length - 13, 12);

        // Left pad the result with zeros to make it 16 characters long
        string panBlock = $"0000{pan12Digits}";

        return panBlock;
    }

    private static string XorHexStrings(string hex1, string hex2)
    {
        // Convert the hex strings to byte arrays
        byte[] bytes1 = Enumerable.Range(0, hex1.Length)
                                  .Where(x => x % 2 == 0)
                                  .Select(x => Convert.ToByte(hex1.Substring(x, 2), 16))
                                  .ToArray();

        byte[] bytes2 = Enumerable.Range(0, hex2.Length)
                                  .Where(x => x % 2 == 0)
                                  .Select(x => Convert.ToByte(hex2.Substring(x, 2), 16))
                                  .ToArray();

        // XOR the bytes
        byte[] xorResult = new byte[bytes1.Length];
        for (int i = 0; i < bytes1.Length; i++)
        {
            xorResult[i] = (byte)(bytes1[i] ^ bytes2[i]);
        }

        // Convert the XOR result back to a hex string
        return BitConverter.ToString(xorResult).Replace("-", "");
    }
}
