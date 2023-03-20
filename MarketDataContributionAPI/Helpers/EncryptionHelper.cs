using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MarketDataContributionAPI.Helpers;

public static class EncryptionHelper
{
    public static string Encrypt(string key, String iv, string plainText)
    {
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            byte[] keyByte = Convert.FromBase64String(key);
            byte[] ivByte = Convert.FromBase64String(iv);

            aes.Key = keyByte;
            aes.IV = ivByte;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    public static string Decrypt(string key, String iv, string cipherText)
    {
        byte[] keyByte = Convert.FromBase64String(key);
        byte[] ivByte = Convert.FromBase64String(iv);

        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyByte;
            aes.IV = ivByte;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}