using Microsoft.EntityFrameworkCore;
using ProductServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Data
{
    public class ProductServicesContext: DbContext
    {
        public ProductServicesContext()
        {

        }

        public ProductServicesContext(DbContextOptions<ProductServicesContext> options): base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductHasSize> ProductHasSizes { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Subcategory> Subcategories { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyValue> PropertyValues { get; set; }
        public virtual DbSet<ProductHasProperty> ProductHasProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasOne(p => p.Subcategory).WithMany(s => s.Products); 
            builder.Entity<Product>().HasMany(p => p.ProductHasSizes).WithOne(p => p.Product);
            builder.Entity<Size>().HasMany(s => s.ProductHasSizes).WithOne(p => p.Size);
            builder.Entity<Price>().HasMany(p => p.Products).WithOne(p => p.Price);
            builder.Entity<Category>().HasMany(c => c.Subcategories).WithOne(s => s.Category);
            builder.Entity<Product>().HasMany(p => p.ProductHasProperties).WithOne(p => p.Product);
            builder.Entity<Property>().HasMany(p => p.PropertyValues).WithOne(p => p.Property);
            builder.Entity<Property>().HasMany(p => p.ProductHasProperties).WithOne(p => p.Property); 

            builder.SeedAsync().Wait(); 
        }
        

    }
}
