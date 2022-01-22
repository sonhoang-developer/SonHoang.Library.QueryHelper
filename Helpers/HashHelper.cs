using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class HashHelper
    {
        public static string MD5Hash(this string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text)); // Compute hash from the bytes of text
            byte[] result = md5.Hash; // Get hash result after compute it  
            StringBuilder strBuilder = new();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2")); // Change it into 2 hexadecimal digits
            }
            return strBuilder.ToString();
        }
        public static string SHA512Hash(this string text)
        {
            SHA512 sha = SHA512.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = sha.ComputeHash(inputBytes);
            // Convert the byte array to hexadecimal string
            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
