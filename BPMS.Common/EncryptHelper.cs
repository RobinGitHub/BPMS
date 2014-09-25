using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.Security.Cryptography;

namespace BPMS.Common
{
    /// <summary>
    /// 加密 解密帮助类
    /// </summary>
    public class EncryptHelper
    {
        private readonly static CryptographyManager crypt = EnterpriseLibraryContainer.Current.GetInstance<CryptographyManager>();

        private const string symmProvider = "SymmProvider";
        private const string hashProvider = "HashProvider";

        /// <summary>
        /// 对称加密(Dpapi)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            return crypt.EncryptSymmetric(symmProvider, text);
        }

        /// <summary>
        /// 解密(Dpapi)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            return crypt.DecryptSymmetric(symmProvider, text);
        }

        /// <summary>
        /// 创建hash(不可逆)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CreateHash(string text)
        {
            return crypt.CreateHash(hashProvider, text);
        }

        public static byte[] CreateHash(byte[] stream)
        {
            return crypt.CreateHash(hashProvider, stream);
        }

        /// <summary>
        /// 比较Hash
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hashText"></param>
        /// <returns></returns>
        public static bool CompareHash(string text, string hashText)
        {
            return crypt.CompareHash(hashProvider, text, hashText);
        }

        public static bool CompareHash(byte[] stream, byte[] hashStream)
        {
            return crypt.CompareHash(hashProvider, stream, hashStream);
        }

        public static string Md5(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            byte[] hashvalue = (new MD5CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hashvalue).Replace("-", "");
        }
    }
}
