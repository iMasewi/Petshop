using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByCustomerIdAsync(int customerId);
    }
}
