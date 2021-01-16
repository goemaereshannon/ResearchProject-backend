using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductServices.Data;
using ProductServices.DTOs;
using ProductServices.Models;
using ProductServices.Repositories;

namespace ProductServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ProductServicesContext context;
        private readonly IGenericRepo<Cart> cartGenericRepo;
        private readonly IGenericRepo<CartProduct> cartProductGenericRepo;
        private readonly IMapper mapper;
        private readonly IProductRepo productRepo;

        public CartController(ProductServicesContext context, IGenericRepo<Cart> cartGenericRepo, IGenericRepo<CartProduct> cartProductGenericRepo, IMapper mapper, IProductRepo productRepo )
        {
            this.context = context;
            this.cartGenericRepo = cartGenericRepo;
            this.cartProductGenericRepo = cartProductGenericRepo;
            this.mapper = mapper;
            this.productRepo = productRepo;
        }

        [HttpGet("api/cartitems/{userId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCartItemsByUserId(Guid userId)
        {
            Cart cart;
            List<Product> products = new List<Product>(); 
            try
            {
                IEnumerable<Cart> carts = await cartGenericRepo.GetByExpressionAsync(c => c.UserId == userId);
                cart = carts.FirstOrDefault();
                IEnumerable<CartProduct> cartProducts = await cartProductGenericRepo.GetByExpressionAsync(cp => cp.CartId == cart.Id);

                foreach (CartProduct item in cartProducts)
                {
                    Product product = await productRepo.GetAsyncByGuid(item.ProductId);
                    products.Add(product);
                } 
            }

            catch(Exception e)
            {
                return NotFound(new { message = "Cart not found" + e });
            }
            IEnumerable<Product> allProducts = products.ToArray();
            var productDTO = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(allProducts);
            return Ok(productDTO);
        }
        [HttpGet("api/cart/{userId}")]
        public async Task<ActionResult<CartDTO>> GetCartByUserId(Guid userId)
        {
            Cart cart;
            try
            {
                IEnumerable<Cart> carts = await cartGenericRepo.GetByExpressionAsync(c => c.UserId == userId);
                cart = carts.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            var cartDTO = mapper.Map<Cart, CartDTO>(cart);
            return Ok(cartDTO); 

        }
    }
}
