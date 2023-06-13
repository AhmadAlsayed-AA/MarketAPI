using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Data.Orders;
using Market.Repository;
using Microsoft.EntityFrameworkCore;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<Order>> GetAll();
        Task<Order> GetById(int id);
        Task<List<Order>> GetAllOrdersByUserId(int userId);
        Task<List<Order>> GetActiveOrdersByUserId(int userId);
        Task Create(Order order);
        Task Update(int id, Order updatedOrder);
        Task Delete(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly MarketContext _context;

        public OrderService(MarketContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders
        .Include(o => o.Customer)
        .Include(o => o.Courier)
        .Include(o => o.Store)
        .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
        .ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Courier)
                .Include(o => o.Store)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }


        public async Task<List<Order>> GetAllOrdersByUserId(int userId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Courier)
                .Include(o => o.Store)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Where(o => o.Customer.UserId == userId || o.Courier.UserId == userId || o.Store.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetActiveOrdersByUserId(int userId)
        {
            var activeStatuses = new[] { DeliveryStatus.Delivered, DeliveryStatus.Cancelled };

            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Courier)
                .Include(o => o.Store)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Where(o => (o.Customer.UserId == userId || o.Courier.UserId == userId || o.Store.UserId == userId) &&
                            o.OrderStatuses.Any(os => !activeStatuses.Contains(os.Status)))
                .ToListAsync();
        }

        public async Task Create(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Order updatedOrder)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            // Update properties of the order
            order.OrderDate = updatedOrder.OrderDate;
            order.TotalAmount = updatedOrder.TotalAmount;
            order.PaymentDetails = updatedOrder.PaymentDetails;
            // Update other properties as needed

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
