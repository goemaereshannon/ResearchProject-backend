using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class ProductHasSizeDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Stock { get; set; }

        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }

        public SizeDTO Size { get; set; }
    }
}

