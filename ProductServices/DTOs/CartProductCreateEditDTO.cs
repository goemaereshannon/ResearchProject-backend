using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class CartProductCreateEditDTO
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
    }
}
