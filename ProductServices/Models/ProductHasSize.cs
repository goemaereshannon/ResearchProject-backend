using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class ProductHasSize
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Stock { get; set; }

        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }

        public Product Product { get; set; }
        public Size Size { get; set; }

    }
}
