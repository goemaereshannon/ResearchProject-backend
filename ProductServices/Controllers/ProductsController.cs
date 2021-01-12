﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductServices.Data;
using ProductServices.DTOs;
using ProductServices.Models;
using ProductServices.Repositories;

namespace ProductServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductServicesContext _context;
        private readonly IGenericRepo<Product> genericProductRepo;
        private readonly IProductRepo productRepo;
        private readonly IGenericRepo<Category> genericCategoryRepo;
        private readonly IGenericRepo<Subcategory> genericSubcategoryRepo;
        private readonly IMapper mapper;

        public ProductsController(ProductServicesContext context, IProductRepo productRepo, IGenericRepo<Category> genericCategoryRepo, IGenericRepo<Subcategory> genericSubcategoryRepo  ,IMapper mapper)
        {
            _context = context;
            this.productRepo = productRepo;
            this.genericCategoryRepo = genericCategoryRepo;
            this.genericSubcategoryRepo = genericSubcategoryRepo;
            this.mapper = mapper;
        }

        // GET: api/products
        /// <summary>
        /// Get all products orderded by newest first. 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/products")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            IEnumerable<Product> products;
            try
            {
                products = await productRepo.GetAllAsync(); 
            }
            catch(Exception exc)
            {
                return NotFound(new { message = "Products not found " + exc }); 
            }
            var productDTO = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products); 
            return Ok(productDTO); 
        }

        // GET: api/products/guid
        [HttpGet("/api/products/{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            try
            {
                if(id == Guid.Empty)
                {
                    return BadRequest(new { message = "Id is empty" }); 
                }

                Product product = await productRepo.GetAsyncByGuid(id); 
                if(product == null || product.PriceId == null)
                {
                    return NotFound(new { message = "Product not found" }); 
                }
                ProductDTO productDTO = mapper.Map<Product, ProductDTO>(product);
                return Ok(productDTO); 
            }
            catch (Exception ex)
            {
                return NotFound(new { message = $"Product not found {ex}" }); 
                throw;
            }
        }

        // GET: api/products/guid
        [HttpGet("/api/productsearch")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductBySearch([FromQuery] string search)
        {
            search = Uri.UnescapeDataString(search); 
            IEnumerable<Product> products;
            IEnumerable<ProductDTO> results; 
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    try
                    {
                        products = await productRepo.GetAllAsync();
                    }
                    catch (Exception exc)
                    {
                        return NotFound(new { message = "Products not found " + exc });
                    }
                    
                }
                else
                {
                    try
                    {
                        products = await productRepo.GetByExpressionAsync(pr =>
                            pr.Subcategory.Name.Contains(search) ||
                            pr.Name.Contains(search) ||
                            pr.Brand.Contains(search) || 
                            pr.Subcategory.Category.Name.Contains(search)
                            ) ;
                    }
                    catch (Exception exc)
                    {

                        return NotFound(new { message = "No product found with searchterm " + search +" Reason: " + exc  });
                    }
                }
                results = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                return Ok(results); 
                
            }
            catch (Exception ex)
            {
                return NotFound(new { message = $"Product not found {ex}" });
                throw;
            }
        }

        [HttpGet("/api/products/category")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory([FromQuery] string category, string subcategory)
        {

            try
            {
                IEnumerable<Product> products;
                IEnumerable<ProductDTO> results; 

                //geen (sub)category meegegeven of category is Nieuw 
                if (!string.IsNullOrEmpty(category) || !string.IsNullOrEmpty(subcategory) || category != "Nieuw")
                {
                    category = Uri.UnescapeDataString(category);
                    subcategory = Uri.UnescapeDataString(subcategory);
                    try
                    {
                        if (!string.IsNullOrEmpty(subcategory))
                        {
                            try
                            {
                                products = await productRepo.GetByExpressionAsync(pr => pr.Subcategory.Name == subcategory);
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                        //category meegegeven 
                        else if (!string.IsNullOrEmpty(category))
                        {
                            try
                            {
                                products = await productRepo.GetByExpressionAsync(pr =>
                                           pr.Subcategory.Category.Name == category
                           );

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            results = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                            return Ok(results);
                        }

                    }
                    catch (Exception exc)
                    {

                        return NotFound(new { message = "No product found with category or subcategory " + category + subcategory + " Reason: " + exc });
                    }
                }
                //category en/of subcategory meegegeven 
                try
                {
                    products = await productRepo.GetAllAsync();
                }
                catch (Exception exc)
                {
                    return NotFound(new { message = "Products not found " + exc });
                }

                results = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                return Ok(results);


            }
            catch (Exception ex)
            {
                return NotFound(new { message = $"Product not found {ex}" });
                throw;
            }
        }

        public async Task<ActionResult<SubcategoryDTO>> PostSubcategory([FromBody] SubcategoryDTO subcategoryDTO)
        {
            try
            {
                if(subcategoryDTO == null)
                {
                    return BadRequest(new { message = "No subcategory input" }); 
                }
                Subcategory newSubcategory = new Subcategory(); 

                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ActionResult<CategoryDTO>> PostCategory([FromBody] CategoryDTO categoryDTO) {
            try
            {
                if(categoryDTO == null)
                {
                    return BadRequest(new { message = "No category input" }); 
                }
                Category newCategory = await genericCategoryRepo.Create(mapper.Map<Category>(categoryDTO));
                return Ok(categoryDTO); 
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: api/products
        [HttpPost("api/products")]
        public async Task<ActionResult<ProductCreateEditDTO>> PostProduct([FromBody] ProductCreateEditDTO productDTO)
        {
            try
            {
                if(productDTO == null)
                {
                    return BadRequest(new { message = "No product input" }); 
                }
                Product newProduct = new Product();

                //bestaat subcategory al? 
                var subcategory = await genericSubcategoryRepo.GetByExpressionAsync(pr => pr.Name == productDTO.Subcategoryname);
                Subcategory productSubcategory = subcategory.FirstOrDefault();
                if (productSubcategory == null) //subcategory bestaat nog niet 
                {
                    SubcategoryDTO subDTO = new SubcategoryDTO();
                    subDTO.Name = productDTO.Subcategoryname;
                    subDTO.Description = productDTO.Subcategorydescription;
                    
                   
                    //bestaat category al? 
                    var category = await genericCategoryRepo.GetByExpressionAsync(ca => ca.Name == productDTO.Categoryname);
                    Category productCategory = category.FirstOrDefault(); 
                    if(productCategory == null)// category bestaat nog niet 
                    {
                        CategoryDTO catDTO = new CategoryDTO();
                        catDTO.Name = productDTO.Categoryname;
                        catDTO.Description = productDTO.Categorydescription;
                        await PostCategory(catDTO); 

                    }
                    //post subcategory
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
