using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class HttpClientHelper
    {
        //public static 

        public static async Task<(string responseData, int? responseStatusCode)> SendRequestAsync(this HttpRequestMessage httpRequestMessage, string endpointURL, IDictionary<string, object> headers)
        {
            HttpClient HttpClient = new();
            if (headers != null)
            {
                foreach (KeyValuePair<string, object> header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value.ToString());
                }
            }
            try
            {
                using HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
                int responseStatusCode = (int)httpResponseMessage.StatusCode;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return (await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false), responseStatusCode);
                }
                else
                {
                    return ($"Request to {endpointURL} error! StatusCode = {httpResponseMessage.StatusCode}", responseStatusCode);
                }
            }
            catch (Exception ex)
            {
                return (ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public static (string responseData, int? responseStatusCode) SendRequestWithStringContent(this HttpMethod method, string endpointURL, string? encodingData = null, IDictionary<string, object>? headers = null, string mediaType = "application/json")
        {
            HttpRequestMessage httpRequestMessage = new(method, endpointURL);
            if (encodingData is not null)
            {
                httpRequestMessage.Content = new StringContent(encodingData, Encoding.UTF8, mediaType);
            }
            return httpRequestMessage.SendRequestAsync(endpointURL, headers).Result;
        }

        public static (string responseData, int? responseStatusCode) SendRequestWithFormDataContent(this HttpMethod method, string endpointURL, MultipartFormDataContent? multipartFormData = null, IDictionary<string, object>? headers = null)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, endpointURL);
            if (!(multipartFormData is null))
            {
                httpRequestMessage.Content = multipartFormData;
            }
            return httpRequestMessage.SendRequestAsync(endpointURL, headers).Result;
        }
    }
}
