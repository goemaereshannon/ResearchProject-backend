using Microsoft.EntityFrameworkCore;
using OrderServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Data
{
    public class OrderServicesContext : DbContext
    {


        public OrderServicesContext()
        {
        }
        public OrderServicesContext(DbContextOptions<OrderServicesContext> options) : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>().HasMany(o => o.OrderProducts).WithOne(op => op.Order);
            builder.Entity<OrderProduct>().HasOne(op => op.Order).WithMany(o => o.OrderProducts); 
            builder.SeedAsync().Wait(); 
        }

    }
}
