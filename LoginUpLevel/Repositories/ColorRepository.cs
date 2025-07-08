using LoginUpLevel.Data;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        public ColorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<int> CountColor()
        {
            return _context.Colors.CountAsync();
        }

        public Task<bool> IsColorExistsAsync(int colorId)
        {
            return _context.Colors
                .AnyAsync(c => c.Id == colorId);
        }
    }
}
