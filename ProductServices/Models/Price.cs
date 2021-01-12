using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class Price
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public double Value { get; set; }
        public string Currency { get; set; }
        public float? Discount { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
