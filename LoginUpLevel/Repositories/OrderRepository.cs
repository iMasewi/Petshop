using LoginUpLevel.Data;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            // Fix CS0029 and CS1662 by correcting the lambda expression and using ToListAsync
            return await _context.Orders.Where(order => order.CustomerId == customerId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(int statusId)
        {
            return await _context.Orders.Where(order => order.StatusId == statusId).ToListAsync();
        }
    }
}
