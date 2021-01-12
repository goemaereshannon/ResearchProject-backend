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

        public SubcategoryDTO Subcategory { get; set; }
        public PriceDTO Price { get; set; }

        public ICollection<ProductHasSizeDTO> ProductHasSizes { get; set; }
        public ICollection<ProductHasPropertyDTO> ProductHasProperties { get; set; }
    }
}
