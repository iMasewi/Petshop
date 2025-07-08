using LoginUpLevel.Data;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CartItem>> GetCartItemByCartAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId).ToListAsync();
        }

        public async Task<CartItem> GetCartItemByProductAndCartAsync(int productId, int cartId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.CartId == cartId);
        }

        public async Task<CartItem> GetCartItemByProductAsync(int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId);
        }
    }
}