using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SonHoang.Library.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Controllers
{
    
    public class BaseController : ControllerBase
    {
        protected IActionResult ResponseOk(dynamic dataResponse = null, string messageResponse = null)
        {
            return StatusCode(StatusCodes.Status200OK, new Status200Response { Data = dataResponse, Message = messageResponse});
        }

        protected IActionResult ResponseCreated(dynamic dataResponse = null, string messageResponse = null)
        {
            return StatusCode(StatusCodes.Status201Created, new Status201Response { Data = dataResponse, Message = messageResponse });
        }

        protected IActionResult ResponseNoContent()
        {
            return StatusCode(StatusCodes.Status204NoContent);
        }

        protected IActionResult ResponseBadRequest(string messageResponse = null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Status400Response { Message = messageResponse });
        }

        protected IActionResult ResponseUnauthorized(string messageResponse = null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new Status401Response { Message = messageResponse });
        }

        protected IActionResult ResponseForbidden(string messageResponse = null)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new Status403Response { Message = messageResponse });
        }

        protected IActionResult ResponseInternalServerError(string messageResponse = null, Exception errorData = null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { errorData = errorData, message = messageResponse});
        }
    }
}
