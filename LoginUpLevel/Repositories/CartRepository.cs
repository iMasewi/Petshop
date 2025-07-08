using LoginUpLevel.Data;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Cart> GetCartByCustomerIdAsync(int customerId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }
}
