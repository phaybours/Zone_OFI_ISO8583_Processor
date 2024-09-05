using System;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using Zone_OFI_ISO8583_Processor.Utilities;

public static class ZPKSessionKeyHandler
{
    
    public static (string clearZPK, string valKcv) DecryptZPKSessionKey(string encryptedZMK, string encryptedZPKSessionKey)
    {
        // Split Encrypted ZPK Session Key into two parts (A and B)
        string zpkPartA = encryptedZPKSessionKey.Substring(0, 16);
        string zpkPartB = encryptedZPKSessionKey.Substring(16, 16);
        string valKCV = encryptedZPKSessionKey.Substring(32, 6);

        // Split Encrypted ZMK into two parts (A and B)
        string zmkPartA = encryptedZMK.Substring(0, 16);
        string zmkPartB = encryptedZMK.Substring(16, 16);

        // Apply Variant 1 (A6 XOR first two characters of ZMK Part B)
        string variantA6 = XORHelper.XorHexStrings("A6", zmkPartB.Substring(0,2));
        string variantedZmkB1 = variantA6 + zmkPartB.Substring(2);
        string variantedZMK1 = zmkPartA + variantedZmkB1;

        // Apply Variant 2 (5A XOR first two characters of ZMK Part B)
        string variantB2 = XORHelper.XorHexStrings("5A", zmkPartB.Substring(0, 2));
        string variantedZmkB2 = variantB2 + zmkPartB.Substring(2);
        string variantedZMK2 = zmkPartA + variantedZmkB2;

        // Convert strings to byte arrays
        byte[] zpkPartABytes = TripleDESHelper.HexStringToByteArray(zpkPartA);
        byte[] zpkPartBBytes = TripleDESHelper.HexStringToByteArray(zpkPartB);
        byte[] variantedZMK1Bytes = TripleDESHelper.HexStringToByteArray(variantedZMK1);
        byte[] variantedZMK2Bytes = TripleDESHelper.HexStringToByteArray(variantedZMK2);

        // Decrypt ZPK Part A with Varianted ZMK 1
        byte[] decryptedPartABytes = TripleDESHelper.TripleDesDecrypt(zpkPartABytes, variantedZMK1Bytes);

        // Decrypt ZPK Part B with Varianted ZMK 2
        byte[] decryptedPartBBytes = TripleDESHelper.TripleDesDecrypt(zpkPartBBytes, variantedZMK2Bytes);

        // Concatenate decrypted parts to get the final Clear ZPK Session Key
        byte[] clearZPKSessionKeyBytes = new byte[decryptedPartABytes.Length + decryptedPartBBytes.Length];
        Buffer.BlockCopy(decryptedPartABytes, 0, clearZPKSessionKeyBytes, 0, decryptedPartABytes.Length);
        Buffer.BlockCopy(decryptedPartBBytes, 0, clearZPKSessionKeyBytes, decryptedPartABytes.Length, decryptedPartBBytes.Length);

        return (TripleDESHelper.ByteArrayToHexString(clearZPKSessionKeyBytes), valKCV);
    }
    
    public static string GenerateKCV(string clearZPKSessionKey)
    {
        byte[] keyBytes = TripleDESHelper.HexStringToByteArray(clearZPKSessionKey);
        byte[] kcvBytes = new byte[8];

        using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
        {
            tdes.Key = keyBytes;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.None;

            using (ICryptoTransform encryptor = tdes.CreateEncryptor())
            {
                kcvBytes = encryptor.TransformFinalBlock(new byte[8], 0, 8);
            }
        }

        return TripleDESHelper.ByteArrayToHexString(kcvBytes).Substring(0, 6);
    }
}
