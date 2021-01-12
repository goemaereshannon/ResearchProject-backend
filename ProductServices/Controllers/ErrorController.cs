using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductServices.Models;

namespace ProductServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
            [HttpGet("{statusCode?}/{errorMessage?}", Name = "GetErrorInfo")]
            public IActionResult HandleErrorCode(int statusCode, string errorMessage)
            {
                //400 request errors 
                var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                //500 server erros 
                IExceptionHandlerFeature feature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
                var reExecuteFeature = feature as ExceptionHandlerFeature;
                var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

                var exceptionMessage = new ExceptionMessage();

                if (statusCode != null)
                {
                    switch (statusCode)
                    {
                        case int n when (statusCode >= 400 && statusCode < 500):
                            exceptionMessage = new ExceptionMessage()
                            {
                                Error = "Request Error",
                                Message = $"Your request contains an error with status code {statusCode}. {errorMessage}",
                                ErrorRoute = (statusCodeData?.OriginalPath != null) ? "Your route: " + (statusCodeData?.OriginalPath != null) : null
                            };
                            break;
                        case int n when (statusCode >= 500 && statusCode < 600):
                            exceptionMessage = new ExceptionMessage()
                            {
                                Message = "Server side error, contact administrator"
                            };
                            break;
                    }
                }
                var result = JsonConvert.SerializeObject(exceptionMessage, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                return StatusCode(statusCode, result);
            }
        }
    
}
