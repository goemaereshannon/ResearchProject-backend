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

        [HttpGet("api/cartitemIds/{userId}")]
        public async Task<ActionResult<List<string>>> GetCartItemIdsByUserId(Guid userId)
        {
            Cart cart;
            List<string> products = new List<string>();
            try
            {
                IEnumerable<Cart> carts = await cartGenericRepo.GetByExpressionAsync(c => c.UserId == userId);
                cart = carts.FirstOrDefault();
                IEnumerable<CartProduct> cartProducts = await cartProductGenericRepo.GetByExpressionAsync(cp => cp.CartId == cart.Id);

                foreach (CartProduct item in cartProducts)
                {
                    Product product = await productRepo.GetAsyncByGuid(item.ProductId);
                    products.Add(product.Id.ToString());
                }
            }

            catch (Exception e)
            {
                return NotFound(new { message = "Cart not found" + e });
            };
            return Ok(products);
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
            if(cart == null)
            {
                PostCart(userId);
                var allCarts = await cartGenericRepo.GetByExpressionAsync(c => c.UserId == userId);
                cart = allCarts.FirstOrDefault();
            }
            var cartDTO = mapper.Map<Cart, CartDTO>(cart);
            return Ok(cartDTO); 

        }
        [HttpPost("api/cart")]
        public async Task<ActionResult<CartDTO>> PostCart(Guid userId)
        {
            Cart cart = new Cart();
            try
            {
                cart.UserId = userId;
                await cartGenericRepo.Create(cart);
                return Ok(cart); 

            }
            catch (Exception exc)
            {
                return RedirectToAction("HandleErrorCode", "Error", new
                {
                    statusCode = 400,
                    errorMessage = $"Creating cart failed {exc}"
                });
                throw;
            }
        }

        [HttpPost("api/cartproduct")]
        public async Task<ActionResult<CartProductDTO>> PostProductToCart(CartProductCreateEditDTO dto)
        {
            Guid userId = Guid.NewGuid();
            Guid productId = Guid.NewGuid(); 
            if (ModelState.IsValid) { 
            userId = new Guid(dto.UserId);
            productId = new Guid(dto.ProductId);
            }
            CartProduct cartProduct = new CartProduct();
            Product product = new Product(); 
            try
            {
                //Zoek of user al cart heeft 
                var carts = await cartGenericRepo.GetByExpressionAsync(c => c.UserId == userId);
                Cart cart = carts.FirstOrDefault(); 
                product = await productRepo.GetAsyncByGuid(productId);
                if (product != null && cart != null)
                {
                    cartProduct.CartId = cart.Id;
                    cartProduct.ProductId = product.Id;
                    cart.TotalItems += 1;
                    cart.TotalPrice += product.Price.Value;
                    await cartProductGenericRepo.Create(cartProduct);
                    await cartGenericRepo.Update(cart, cart.Id); 
                    return Created("api/cart", cartProduct); 
                }
                else if(cart == null){
                   await PostCart(userId);
                    var allCarts = await cartGenericRepo.GetByExpressionAsync(c => c.UserId == userId);
                    Cart thisCart = allCarts.FirstOrDefault();
                    if (product != null && thisCart != null)
                    {
                        cartProduct.CartId = thisCart.Id;
                        cartProduct.ProductId = product.Id;
                        thisCart.TotalItems += 1;
                        thisCart.TotalPrice += product.Price.Value;
                        await cartProductGenericRepo.Create(cartProduct);
                        await cartGenericRepo.Update(thisCart, thisCart.Id); 
                        return Created("api/cart", cartProduct);
                    }
                    else
                    {
                        return NotFound(new { message = "Cart or product not found" });
                    }
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
            Cart cart = await cartGenericRepo.GetAsyncByGuid(cartId); 
            cart.TotalItems = cart.TotalItems -  1;
            cart.TotalPrice = cart.TotalPrice - product.Price.Value;
            await cartGenericRepo.Update(cart, cartId) ;
            var cartTest = await cartGenericRepo.GetAsyncByGuid(cartId); 
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

        [HttpDelete("api/cart/{cartId}")]
        public async Task<ActionResult> DeleteAllProductsFromCart(Guid cartId)
           {
            // ophalen alle producten uit tussentabel met cartId gegeven cartId
            var products = await cartProductGenericRepo.GetByExpressionAsync(cp => cp.CartId == cartId);
             if (products == null) // geen producten in cart => return
            {
                return BadRequest();
            }
            foreach (var item in products) //voor elk product in de tussentabel => product verwijderen 
            {
                await cartProductGenericRepo.Delete(item); 
            }

            Cart cart = await cartGenericRepo.GetAsyncByGuid(cartId);
            cart.TotalItems -= cart.TotalItems;
            cart.TotalPrice -= cart.TotalPrice;
            await cartGenericRepo.Update(cart, cartId);
            try
            {
                await cartProductGenericRepo.SaveAsync();
                await cartGenericRepo.SaveAsync(); 
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
