using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class ProductCreateEditDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public string Subcategoryname { get; set; }
        public string Subcategorydescription { get; set; }
        public string Categoryname { get; set; }
        public string Categorydescription { get; set; }
        public PriceDTO Price { get; set; }

        public ICollection<ProductHasSizeDTO> ProductHasSizes { get; set; }
        public ICollection<ProductHasPropertyDTO> ProductHasProperties { get; set; }
    }
}
