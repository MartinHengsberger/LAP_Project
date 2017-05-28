using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CardGame.DAL.Logic
{
    public class Helper
    {
        /// <summary>
        /// Generates SHA512 Hash for password
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GenerateHash(string s)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(s);
            using (SHA512 sha = new SHA512Managed())
            {
                var hash = sha.ComputeHash(bytes);
                return GetHexNotation(hash);
            }        
        }

        /// <summary>
        /// Generates Salt for password
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            var salt = new byte[64];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);

            return GetHexNotation(salt);
        }

        /// <summary>
        /// Get Hexnotation
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string GetHexNotation(byte[] bytes)
        {
            var hashStringBuilder = new StringBuilder(128);

            foreach (var b in bytes)
            {
                hashStringBuilder.Append(b.ToString("X2"));
            }

            return hashStringBuilder.ToString();
        }

    }
}
