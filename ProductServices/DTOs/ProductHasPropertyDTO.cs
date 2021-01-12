using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class ProductHasPropertyDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid PropertyId { get; set; }
        public PropertyDTO Property { get; set; }
    }
}
