using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentDTO commentDto);
        Task UpdateCommentAsync(CommentDTO commentDto, int customerId);
        Task DeleteCommentAsync(int id);
        Task<IEnumerable<CommentDTO>> GetCommentsByProductIdAsync(int productId);
        Task<IEnumerable<CommentDTO>> GetCommentsByRatingAsync(int productId,int rating);
    }
}
