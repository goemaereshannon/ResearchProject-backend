using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public Guid UserId { get; set; }
        public string Status { get; set; } = "Bestelling wordt verwerkt";
        public double TotalPrice { get; set; } = 0; 
        public DateTime ShippingDate { get; set; } = DateTime.Now; 
        public DateTime PlacementDate { get; set; } = DateTime.Now;
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
