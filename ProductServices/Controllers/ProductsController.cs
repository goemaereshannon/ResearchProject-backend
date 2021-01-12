using System;
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
        private readonly IMapper mapper;

        public ProductsController(ProductServicesContext context, IProductRepo productRepo, IMapper mapper)
        {
            _context = context;
            this.productRepo = productRepo;
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


        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
