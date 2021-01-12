using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ExceptionMessage
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public string ErrorRoute { get; set; } = null; 
    }
}
