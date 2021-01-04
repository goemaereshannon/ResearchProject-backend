using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Name { get; set; }
        public bool ProductAvailable { get; set; } = true;

        public Guid? CategoryId { get; set; }
        public Guid? PriceId { get; set; }

        public Category Category { get; set; }
        public Price Price { get; set; }

        public ICollection<ProductHasSize> ProductHasSizes { get; set; }

    }
}
