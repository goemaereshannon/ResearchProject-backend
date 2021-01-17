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

        [HttpPost("api/cart/{cartId}/{productId}")]
        public async Task<ActionResult<CartProductDTO>> PostProductToCart(Guid cartId, Guid productId)
        {
            CartProduct cartProduct = new CartProduct();
            Product product = new Product(); 
            try
            {
                Cart cart = await cartGenericRepo.GetAsyncByGuid(cartId) ; 
                product = await productRepo.GetAsyncByGuid(productId);
                if (product != null && cart != null)
                {
                    cartProduct.CartId = cart.Id;
                    cartProduct.ProductId = product.Id;
                    cartProduct.TotalItems += 1;
                    cartProduct.TotalPrice += product.Price.Value;
                    await cartProductGenericRepo.Create(cartProduct);
                    return Created("api/cart", cartProduct); 
                }
                else
                {
                    return NotFound(new { message = "Cart or product not found" });
                }
            }
            catch (Exception exc)
            {
                return RedirectToAction("HandleErrorCode", "Error", new
                {
                    statusCode = 400,
                    errorMessage = $"Adding product to cart failed {exc}"
                });
                throw;
            }

        }
        [HttpDelete("api/cart/{cartId}/{productId}")]
        public async Task<ActionResult> DeleteProductFromCart(Guid cartId, Guid productId)
        {

            var products = await cartProductGenericRepo.GetByExpressionAsync(cp => cp.CartId == cartId && cp.ProductId == productId);
            CartProduct cartProduct = products.FirstOrDefault();
            Product product = await productRepo.GetAsyncByGuid(productId); 
            if(cartProduct == null)
            {
                return BadRequest(); 
            }
            cartProduct.TotalItems -= 1;
            cartProduct.TotalPrice -= product.Price.Value;
            await cartProductGenericRepo.Delete(cartProduct);
            try
            {
                await cartProductGenericRepo.SaveAsync();
            }
            catch (Exception ex)
            {
                return RedirectToAction("HandleErrorCode", "Error", new
                {
                    statusCode = 400,
                    errorMessage = $"Deleting product from cart failed {ex}"
                });
                throw;
            }
            return NoContent(); 
        }
      
    }
}
