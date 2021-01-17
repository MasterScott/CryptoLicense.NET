using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLicense.NET.Helpers
{
    public class Cryptography
    {
        internal static Random random = new Random();
        public static string KEY { get; set; }
        public static string IV { get; set; }

        public static string createMD5(string stringToHash)
        {
            string result;
            using (var md = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(stringToHash);
                var value = md.ComputeHash(bytes);
                result = BitConverter.ToString(value).Replace("-", string.Empty).ToLowerInvariant();
            }

            return result;
        }


        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(KEY) ||
                string.IsNullOrEmpty(IV))
                throw new Exception("You must include the communication & encryption key!");

            return EncryptString(value, SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(KEY)))), Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(IV))));
        }

        public static string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(KEY) ||
                string.IsNullOrEmpty(IV))
                throw new Exception("You must include the communication & encryption key!");

            return DecryptString(value, SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(KEY)))), Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(IV))));
        }

        private static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            using (var encryptor = Aes.Create())
            {
                encryptor.Mode = CipherMode.CBC;
                encryptor.Key = key;
                encryptor.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    using (var aesEncryptor = encryptor.CreateEncryptor())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write))
                        {

                            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] cipherBytes = memoryStream.ToArray();

                            return Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
                        }
                    }
                }
            }
        }

        private static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            using (var encryptor = Aes.Create())
            {
                encryptor.Mode = CipherMode.CBC;
                encryptor.Key = key;
                encryptor.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    using (var aesDecryptor = encryptor.CreateDecryptor())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write))
                        {

                            byte[] cipherBytes = Convert.FromBase64String(cipherText);

                            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] plainBytes = memoryStream.ToArray();

                            return Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
                        }
                    }
                }
            }
        }
    }
}
