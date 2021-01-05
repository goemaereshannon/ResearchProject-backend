using Microsoft.EntityFrameworkCore;
using ProductServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Data
{
    public static class ProductServicesExt
    {
        public static ProductServicesContext _context { get; set; }

        public static async Task SeedAsync(this ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<Category>().HasData(_categoriesData);
                modelBuilder.Entity<Subcategory>().HasData(_subCategoriesData);
                modelBuilder.Entity<Price>().HasData(_pricesData);
                modelBuilder.Entity<Size>().HasData(_sizesData);
                modelBuilder.Entity<ProductHasSize>().HasData(_productHasSizesData);
                modelBuilder.Entity<Product>().HasData(_productsData); 
            }
            catch(Exception e)
            {
                Debug.WriteLine(e); 
                throw; 
            }
        }

        //testdata
        private readonly static List<Category> _categoriesData = new List<Category>()
        {
            new Category
            {
                Id = new Guid("9E17AF7B-DF05-4C69-94B8-586659C7152F"),
                Name = "Beauty",
                Description = "Make-up, perfume, hair care and skin care",
            }
        };

        private readonly static List<Subcategory> _subCategoriesData = new List<Subcategory>
        {
            new Subcategory
            {
                Id= new Guid("79A86C66-F7E4-4BC4-9E01-525AD78754BD"),
                Name = "Body Lotion", 
                Description = "Body lotion or body milk is a thick fluid oil-in-water emulsion which is applied on the skin to hydrate", 
                CategoryId = new Guid("9E17AF7B-DF05-4C69-94B8-586659C7152F"),
            }
        };



        private readonly static List<Price> _pricesData = new List<Price>
        {
            new Price
            {
                Id= new Guid("628B89E3-FFD4-4659-BA39-C67EEE11672B"),
                Value = 30.75,
                Currency = "€"
            }
        };

        private readonly static List<Product> _productsData = new List<Product>
        {
            new Product
            {
                Id=  new Guid("d470123f-7795-4158-aa2b-9088e29de88d"),
                Name = "Repair Almond",
                Description = "Body Lotion for dry, sun damaged skin. Almond scented",
                ProductAvailable = true,
                Brand = "YSL",
                SubcategoryId = new Guid("79A86C66-F7E4-4BC4-9E01-525AD78754BD"),
                PriceId = new Guid("628B89E3-FFD4-4659-BA39-C67EEE11672B"),

            }
        };

        private readonly static List<Size> _sizesData = new List<Size>()
        {
            new Size
            {
                Id =  new Guid("33478a6b-437f-4416-932d-638b1c0415ea"),
                Name = "M", 
            }
        };

        private readonly static List<ProductHasSize> _productHasSizesData = new List<ProductHasSize>
        {
            new ProductHasSize
            {
                Id= new Guid("628B89E3-FFD4-4659-BA39-C67EEE11672B"),
                ProductId = new Guid("d470123f-7795-4158-aa2b-9088e29de88d"),
                SizeId =  new Guid("33478a6b-437f-4416-932d-638b1c0415ea"),
                Stock = 10
            }
        };
    }
}
