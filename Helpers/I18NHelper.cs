using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SonHoang.Library.Helpers
{
    public static class I18NHelper
    {
        public static string Translate(this string text, string fromLanguage = "en", string toLanguage = "vi")
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(text)}";
            WebClient webClient = new()
            {
                Encoding = Encoding.UTF8
            };
            JArray data = JArray.Parse(webClient.DownloadString(url));
            string resultTranslate = data[0][0][0].ToString();
            return resultTranslate;
        }
    }
}
