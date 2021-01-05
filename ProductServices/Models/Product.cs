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
        public string Brand { get; set; }
        public string Description { get; set; }
        public bool ProductAvailable { get; set; } = true;

        public Guid? SubcategoryId { get; set; }
        public Guid? PriceId { get; set; }

        public Subcategory Subcategory { get; set; }
        public Price Price { get; set; }

        public ICollection<ProductHasSize> ProductHasSizes { get; set; }

    }
}
