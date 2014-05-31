using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Owin.Security.DataProtection;

namespace BombVacuum.Providers
{
    public class AesDataProtectorProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new AesDataProtector(SeedHash);
        }

        private string SeedHash
        {
            get
            {
                var key = ConfigurationManager.AppSettings["DataProviderKey"];
                if (string.IsNullOrWhiteSpace(key))
                {
                    key = HashString(Environment.MachineName);
                }
                return key;
            }
        }

        private string HashString(string value)
        {
            var alg = SHA512.Create();
            var result = alg.ComputeHash(Encoding.ASCII.GetBytes(value));
            return HexStringFromBytes(result).ToUpper();
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
    internal class AesDataProtector : IDataProtector
    {
        #region Fields

        private byte[] key;

        #endregion Fields

        #region Constructors

        public AesDataProtector(string key)
        {
            using (var sha1 = new SHA256Managed())
            {
                this.key = sha1.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        #endregion Constructors

        #region IDataProtector Methods

        public byte[] Protect(byte[] data)
        {
            byte[] dataHash;
            using (var sha = new SHA256Managed())
            {
                dataHash = sha.ComputeHash(data);
            }

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = this.key;
                aesAlg.GenerateIV();

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, 16);

                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var bwEncrypt = new BinaryWriter(csEncrypt))
                    {
                        bwEncrypt.Write(dataHash);
                        bwEncrypt.Write(data.Length);
                        bwEncrypt.Write(data);
                    }
                    var protectedData = msEncrypt.ToArray();
                    return protectedData;
                }
            }
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = this.key;

                using (var msDecrypt = new MemoryStream(protectedData))
                {
                    byte[] iv = new byte[16];
                    msDecrypt.Read(iv, 0, 16);

                    aesAlg.IV = iv;

                    using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var brDecrypt = new BinaryReader(csDecrypt))
                    {
                        var signature = brDecrypt.ReadBytes(32);
                        var len = brDecrypt.ReadInt32();
                        var data = brDecrypt.ReadBytes(len);

                        byte[] dataHash;
                        using (var sha = new SHA256Managed())
                        {
                            dataHash = sha.ComputeHash(data);
                        }

                        if (!dataHash.SequenceEqual(signature))
                            throw new SecurityException("Signature does not match the computed hash");

                        return data;
                    }
                }
            }
        }

        #endregion IDataProtector Methods
    }
}