using Microsoft.EntityFrameworkCore;
using OrderServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Data
{
    public static class OrderServicesExt
    {
        public static OrderServicesContext _context { get; set; }
        public static async Task SeedAsync(this ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<Order>().HasData(_ordersData);
                modelBuilder.Entity<OrderProduct>().HasData(_orderProductsData);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e); 
                throw;
            }
        }
        //testdata
        private readonly static List<Order> _ordersData = new List<Order>
        {
            new Order
            {
                Id = new Guid("46303583-22ca-4a52-9a44-1ccbb1ef590a"),
                UserId = new Guid("96a30dc9-c01c-45fb-8a50-81baf9b10fca"), 
            }
        };
        private readonly static List<OrderProduct> _orderProductsData = new List<OrderProduct>
        {
            new OrderProduct
            {
                Id = new Guid("c3766a13-ee7f-41fd-8131-42f6aab06575"),
                OrderId = new Guid("46303583-22ca-4a52-9a44-1ccbb1ef590a"),
                ProductId = new Guid("d470123f-7795-4158-aa2b-9088e29de88d"),
            }
        };


    }
}
