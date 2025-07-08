using LoginUpLevel.Data;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class ProductColorRepository : Repository<ProductColor>, IProductColorRepository
    {
        public ProductColorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ProductColor> GetProductColorByProductAndColorAsync(int productId, int colorId)
        {
            return await _context.ProductColors
                .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.ColorId == colorId);
        }

        public async Task<IEnumerable<ProductColor>> GetProductColorsByColorIdAsync(int colorId)
        {
            return await _context.ProductColors
                .Where(pc => pc.ColorId == colorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductColor>> GetProductColorsByProductIdAsync(int productId)
        {
            return await _context.ProductColors
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();
        }

        public async Task<bool> IsProductColorExistsAsync(int productId, int colorId)
        {
            return await _context.ProductColors
                .AnyAsync(pc => pc.ProductId == productId && pc.ColorId == colorId);
        }
    }
}
