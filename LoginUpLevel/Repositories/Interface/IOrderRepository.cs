using LoginUpLevel.DTOs;
using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(int statusId);
    }
}
