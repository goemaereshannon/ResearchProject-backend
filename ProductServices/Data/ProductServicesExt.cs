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
                modelBuilder.Entity<ProductHasProperty>().HasData(_productHasPropertyData);
                modelBuilder.Entity<Property>().HasData(_propertiesData);
                modelBuilder.Entity<PropertyValue>().HasData(_propertyValuesData);
                modelBuilder.Entity<Cart>().HasData(_cartsData);
                modelBuilder.Entity<CartProduct>().HasData(_cartProductsData);
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
                Name = "Cosmetica",
                Description = "Make-up, parfum, haar- en huidverzorging",
            }
        };

        private readonly static List<Subcategory> _subCategoriesData = new List<Subcategory>
        {
            new Subcategory
            {
                Id= new Guid("79A86C66-F7E4-4BC4-9E01-525AD78754BD"),
                Name = "Body lotion", 
                Description = "Body lotion of body milk is een dikke vloeibare olie-in-water emulsie om het lichaam te hydrateren", 
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
                Description = "Body Lotion voor droge, door zon beschadigde huid. Met amandelgeur.",
                ProductAvailable = true,
                Brand = "YSL",
                SubcategoryId = new Guid("79A86C66-F7E4-4BC4-9E01-525AD78754BD"),
                PriceId = new Guid("628B89E3-FFD4-4659-BA39-C67EEE11672B"),
                ImageUrl = "https://images.unsplash.com/photo-1551446339-1e5c6f164ec2?ixid=MXwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHw%3D&ixlib=rb-1.2.1&auto=format&fit=crop&w=676&q=80", 
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

        private readonly static List<Property> _propertiesData = new List<Property>()
        {
            new Property
            {
                Id =  new Guid("fa38c158-cde3-4e01-a3a8-92ff534d2a89"),
                Name = "Scent",
            }
        };

        private readonly static List<PropertyValue> _propertyValuesData = new List<PropertyValue>()
        {
            new PropertyValue
            {
                Id = new Guid("a3a45628-329e-49c6-b495-f8a11acf47ec"), 
                PropertyId =  new Guid("fa38c158-cde3-4e01-a3a8-92ff534d2a89"),
                Value = "Almond"
                
            }
        };

        private readonly static List<ProductHasProperty> _productHasPropertyData = new List<ProductHasProperty>()
        {
            new ProductHasProperty
            {
                Id = new Guid("ff64f462-4303-4204-8f0a-f1500c1f47b6"),
                PropertyId =  new Guid("fa38c158-cde3-4e01-a3a8-92ff534d2a89"), 
                ProductId=  new Guid("d470123f-7795-4158-aa2b-9088e29de88d"),
            }
        };

        private readonly static List<Cart> _cartsData = new List<Cart>
        {
            new Cart
            {
                Id = new Guid("e5698ebe-72a4-40a8-84a3-7e3e4eeeeae2"),
                UserId = new Guid("C051825D-2611-402D-8B29-563DD883848B"),               
                TotalItems = 1,
                TotalPrice = 30.75,
            }
        };
        private readonly static List<CartProduct> _cartProductsData = new List<CartProduct>
        {
            new CartProduct
            {
                Id = new Guid("c3fde068-9168-48ce-8763-cda48d178a9a"),
                ProductId =new Guid("d470123f-7795-4158-aa2b-9088e29de88d"),
                CartId = new Guid("e5698ebe-72a4-40a8-84a3-7e3e4eeeeae2"),

            }
        };
    }
}
