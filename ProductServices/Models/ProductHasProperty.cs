using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class ProductHasProperty
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public Guid ProductId { get; set; }
        public Guid PropertyId { get; set; }
        public Product Product { get; set; }
        public Property Property { get; set; }
    }
}
