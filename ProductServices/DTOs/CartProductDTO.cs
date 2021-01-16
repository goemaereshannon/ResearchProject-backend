using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class CartProductDTO
    {
        [NotMapped]
        private List<ProductDTO> _products = new List<ProductDTO>();
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public CartDTO Cart { get; set; }
        public ProductDTO Product { get; set; }
        public int TotalItems { get; set; } = 0;
        public double TotalPrice { get; set; } = 0.00;
    }
}
