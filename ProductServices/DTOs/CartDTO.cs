using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class CartDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int TotalItems { get; set; } = 0;
        public double TotalPrice { get; set; } = 0.00;
        public ICollection<CartProductDTO> CartProducts { get; set; }
    }
}
