using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(Data.ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId)
        {
            return await _context.Comments
                .Where(c => c.ProductId == productId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Comment>> GetCommentsByRatingAsyns(int productId, int rating)
        {
            return await _context.Comments
                .Where(c => c.Rating == rating && c.ProductId == productId)
                .ToListAsync();
        }
    }
}
