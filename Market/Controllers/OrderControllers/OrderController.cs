using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Market.Data.Orders;
using Market.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetAllOrdersByUserId(userId);
            return Ok(orders);
        }

        [HttpGet("user/{userId}/active")]
        public async Task<ActionResult<List<Order>>> GetActiveOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetActiveOrdersByUserId(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(Order order)
        {
            await _orderService.Create(order);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, Order updatedOrder)
        {
            try
            {
                await _orderService.Update(id, updatedOrder);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}