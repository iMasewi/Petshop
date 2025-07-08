using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<CartItem> GetCartItemByProductAsync(int productId);
        Task<IEnumerable<CartItem>> GetCartItemByCartAsync(int cartId);
        Task<CartItem> GetCartItemByProductAndCartAsync(int productId, int cartId);
    }
}
