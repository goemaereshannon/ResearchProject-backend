using ProductServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public bool ProductAvailable { get; set; } = true;
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public Subcategory Subcategory { get; set; }
        public Price Price { get; set; }

        public ICollection<ProductHasSize> Sizes { get; set; }
        public ICollection<ProductHasProperty> Properties { get; set; }
    }
}
