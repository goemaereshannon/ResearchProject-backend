using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class OrderProduct
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
