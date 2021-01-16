using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class CartProduct
    {
        [NotMapped]
        private List<Product> _products = new List<Product>(); 
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int TotalItems { get; set; } = 0;
        public double TotalPrice { get; set; } = 0.00; 

    }
}
