using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateAsync(OrderDTO orderDto, int customerId);
        Task UpdateStatusAsync(int id, int statusId);
        Task DeleteAsync(int id);
        Task<OrderDTO> GetByIdAsync(int id);
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<IEnumerable<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId);
        Task<IEnumerable<OrderDTO>> GetOrdersByStatusIdAsync(int customerId);
    }
}
