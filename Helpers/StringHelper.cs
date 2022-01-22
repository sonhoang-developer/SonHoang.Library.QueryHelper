using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class StringHelper
    {
        public static string UpperCaseFirstLetter(this string str)
        {
            str = str[..1].ToUpper() + str[1..];
            return str;
        }
        public static string LowerCaseFirstLetter(this string str)
        {
            str = str[..1].ToLower() + str[1..];
            return str;
        }
        public static string ReverseString(this string str)
        {
            char[] myArr = str.ToCharArray();
            Array.Reverse(myArr);
            return new string(myArr);
        }
        public static string ToStringJoin(this IEnumerable<string> listStr, string character=",")
        {
            return string.Join(character, listStr);
        }
    }
}
