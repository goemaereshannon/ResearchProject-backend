using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int TotalItems { get; set; } = 0;
        public double TotalPrice { get; set; } = 0.00;
        public ICollection<CartProduct> CartProducts { get; set; }

    }
}
