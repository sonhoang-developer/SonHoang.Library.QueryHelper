using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class RandomHelper
    {
        public static string RandomString(int numberChar, bool uppercaseStr = false)
        {
            string result = "";
            string b = "abcdefghijklmnopqrstuvwxyz";
            Random ran = new();
            
            for (int i = 0; i < numberChar; i++)
            {
                int a = ran.Next(26);
                result += b.ElementAt(a);
            }
            if (uppercaseStr is true)
            {
                result = result.ToUpper();
            }
            return result;
        }
        public static string RandomNumber(int numberChar)
        {
            string result = "";
            for (int i = 0; i < numberChar; i++)
            {
                result += (new Random()).Next(0, 9).ToString();
            }
            return result;
        }
    }
}
