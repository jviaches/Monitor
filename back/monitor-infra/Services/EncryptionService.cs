using monitor_core.Settings;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace monitor_infra.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string _key;

        public EncryptionService(EncryptionSettings settings)
        {
            _key = settings.Key;
        }

        public string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            byte[] encryptedBites;
            try
            {
                encryptedBites = Convert.FromBase64String(input);
                return DecryptStringFromBytes_Aes(encryptedBites, Encoding.ASCII.GetBytes(_key));
            }
            catch (Exception exc)
            {
                return input;
            }
        }

        public string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var encryptedString = EncryptStringToBytes_Aes(input, Encoding.ASCII.GetBytes(_key));
            return Convert.ToBase64String(encryptedString);
        }

        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] key)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] encrypted;
            byte[] iv;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();
                iv = aesAlg.IV;
                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                encrypted = msEncrypt.ToArray();
            }

            var combinedWithIV = new byte[iv.Length + encrypted.Length];
            Array.Copy(iv, 0, combinedWithIV, 0, iv.Length);
            Array.Copy(encrypted, 0, combinedWithIV, iv.Length, encrypted.Length);

            return combinedWithIV;
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                byte[] iv = new byte[aesAlg.BlockSize/8];
                byte[] cipherValue = new byte[cipherText.Length - iv.Length];

                Array.Copy(cipherText, iv, iv.Length);
                Array.Copy(cipherText, iv.Length, cipherValue, 0, cipherValue.Length);

                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using var msDecrypt = new MemoryStream(cipherValue);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }
    }
}
