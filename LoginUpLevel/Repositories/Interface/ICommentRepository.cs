using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId);
        Task<IEnumerable<Comment>> GetCommentsByRatingAsyns(int productId, int rating);
    }
}
