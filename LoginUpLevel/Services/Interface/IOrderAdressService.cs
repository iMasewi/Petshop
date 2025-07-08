using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface IOrderAdressService
    {
        Task<IEnumerable<OrderAdressDTO>> GetAllOrderAdressAsync();
        Task<OrderAdressDTO> GetOrderAdressByIdAsync(int id);
        Task<OrderAdressDTO> AddOrderAdressAsync(OrderAdressDTO orderAdresssDto);
        Task UpdateOrderAdressAsync(OrderAdressDTO orderAdresss, int id, int customerId);
        Task DeleteOrderAdressAsync(int id, int customerId);
    }
}
