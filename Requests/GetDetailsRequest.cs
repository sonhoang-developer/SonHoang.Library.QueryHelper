using SonHoang.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SonHoang.Library.Requests
{
    public class GetDetailsRequest<T> where T : class
    {
        public List<string>? SelectFields { get; set; } = typeof(T).GetAllPropertiesName(); // Select Fields
    }
}
