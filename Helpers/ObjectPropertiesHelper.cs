using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class ObjectPropertiesHelper
    {
        public static Dictionary<string, string>? ToDictionary(this object data)
        {
            ArgumentNullException.ThrowIfNull(data);
            var json = JsonConvert.SerializeObject(data);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dictionary;
        }
        public static List<string> GetAllPropertiesName(this Type type)
        {
            return type.GetProperties().Select(t => t.Name).ToList();
        }
        public static T MergeObjectHasSameProperties<T>(this object srcData, T destData) where T : class
        {
            ArgumentNullException.ThrowIfNull(destData);
            /*        List<string> srcProperties = srcData.GetType().GetAllPropertiesName();*/
            List<string> destProperties = typeof(T).GetAllPropertiesName();
            foreach (PropertyInfo propertyInfoSrc in srcData.GetType().GetProperties())
            {
                if (propertyInfoSrc.GetValue(srcData, null) != null && destProperties.Any(destProperty => destProperty.Equals(propertyInfoSrc.Name)))
                {
                    //check type
                    if (propertyInfoSrc.PropertyType.Name.Equals(typeof(T).GetProperty(propertyInfoSrc.Name).PropertyType.Name))
                    {
                        destData = destData.SetPropertyValue<T>(propertyInfoSrc.Name, propertyInfoSrc.GetValue(srcData, null));
                        //destData.GetType().GetProperty(propertyInfo.Name).SetValue(destData, propertyInfo.GetValue(srcData, null));
                    }
                }
            }
            return destData;
        }
        public static T SetPropertyValue<T>(this T destData, string propertyName, object value)
        {
            ArgumentNullException.ThrowIfNull(destData);
            ArgumentNullException.ThrowIfNull(propertyName);
            destData.GetType().GetProperty(propertyName).SetValue(destData, value);
            return destData;
        }
        public static JObject LowerCasePropertiesObject(this object data)
        {
            ArgumentNullException.ThrowIfNull(data);

            JObject result = JObject.FromObject(data);
            foreach (var property in result.Properties().ToList())
            {
                if (property.Value.Type == JTokenType.Object)// replace property names in child object
                    LowerCasePropertiesObject((JObject)property.Value);

                property.Replace(new JProperty(property.Name[..1].ToLower() + property.Name[1..], property.Value));// properties are read-only, so we have to replace them
            }
            return result;
        }
    }
}
