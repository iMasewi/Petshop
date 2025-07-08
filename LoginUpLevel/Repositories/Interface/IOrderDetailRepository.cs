using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByProductIdAsync(int productId);
    }
}
