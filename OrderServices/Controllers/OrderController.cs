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
            return Ok(orders);         }

    }
}
