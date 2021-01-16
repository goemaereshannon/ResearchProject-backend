using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int TotalItems { get; set; } = 0;
        public ICollection<CartProduct> CartProducts { get; set; }

    }
}
