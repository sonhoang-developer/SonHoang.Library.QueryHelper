using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Responses
{
    public class Status401Response
    {
        public string Message { get; set; }
        public int StatusCode { get; set; } = StatusCodes.Status401Unauthorized;
    }
}
