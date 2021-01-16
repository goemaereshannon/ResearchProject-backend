using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class CartProduct
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public int ItemQuantity { get; set; }
        public Cart Cart { get; set; }

    }
}
