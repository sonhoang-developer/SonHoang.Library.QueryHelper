using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Responses
{
    public class Status200Response
    {
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; } = StatusCodes.Status200OK;
    }
}
