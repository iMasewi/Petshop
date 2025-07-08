using LoginUpLevel.Data;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetail
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByProductIdAsync(int productId)
        {
            return await _context.OrderDetail
                .Where(od => od.ProductId == productId)
                .ToListAsync();
        }
    }
}
