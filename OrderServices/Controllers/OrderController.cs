using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderServices.Data;
using OrderServices.Models;

namespace OrderServices.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderServicesContext _context;
        private readonly IGenericRepo<Order> genericOrderRepo;
        private readonly IGenericRepo<OrderProduct> genericOrderProductRepo;

        public OrderController(OrderServicesContext context, IGenericRepo<Order> genericOrderRepo, IGenericRepo<OrderProduct> genericOrderProductRepo)
        {
            _context = context;
            this.genericOrderRepo = genericOrderRepo;
            this.genericOrderProductRepo = genericOrderProductRepo;
        }

        [HttpGet("/api/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() { 
            IEnumerable<Order> orders; 
            try 
	        {
                orders = await genericOrderRepo.GetAllAsync();
                foreach (var order in orders)
                {
                    IEnumerable<OrderProduct> orderProducts = await genericOrderProductRepo.GetByExpressionAsync(op => op.OrderId == order.Id);
                    order.OrderProducts = orderProducts.ToList(); 
                }
	        }
	        catch (Exception exc)
	        {
                return NotFound(new { message = "Orders not found " + exc });
            }
            return Ok(orders);         
        }

        [HttpGet("/api/orders/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(Guid userId)
        {
            IEnumerable<Order> orders;
            try
            {
                orders = await genericOrderRepo.GetByExpressionAsync(o => o.UserId == userId);
                foreach (var order in orders)
                {
                    IEnumerable<OrderProduct> orderProducts = await genericOrderProductRepo.GetByExpressionAsync(op => op.OrderId == order.Id);
                    order.OrderProducts = orderProducts.ToList();
                }
            }
            catch (Exception exc)
            {
                return NotFound(new { message = "Orders not found " + exc });
            }
            return Ok(orders);
        }

        [HttpPost("api/order/{userId}")]
        public async Task<ActionResult<OrderProduct>> PostProductsToOrder(Guid userId,[FromBody] List<Guid> productIds, double price)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            Order order = new Order();
            order.UserId = userId;
            order.TotalPrice = price; 
            try
            {
                var orderPost = await genericOrderRepo.Create(order);
                if (orderPost == null)
                {
                    return BadRequest(new { Message = $"Not able to create new order" });
                }
                foreach (var productId in productIds)
                {
                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.OrderId = order.Id; 
                    orderProduct.ProductId = productId;
                    orderProducts.Add(orderProduct);
                    var result = await genericOrderProductRepo.Create(orderProduct); 
                    if(result == null)
                    {
                        return BadRequest(new { Message = $"Product {productId} could not be saved to order" });
                    }
                }
                return Created("api/order/{userId}", orderProducts); 
            }
            catch (Exception e)
            {
                return BadRequest(); 
                throw e;
            }
        }

    }
}
