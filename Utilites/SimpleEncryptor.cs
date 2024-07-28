using System;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SimpleEncryptor
{
    private static readonly byte[] EncryptionKey = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["EncryptionKey"]);
    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = EncryptionKey;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            aes.GenerateIV();
            byte[] iv = aes.IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(iv, 0, iv.Length);

                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);

        byte[] iv = new byte[16];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);

        byte[] cipher = new byte[fullCipher.Length - iv.Length];
        Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        using (Aes aes = Aes.Create())
        {
            aes.Key = EncryptionKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
