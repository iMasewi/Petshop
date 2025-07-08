using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> AddCustomerAsync(CustomerDTO customer);
        Task UpdateCustomerAsync(CustomerDTO customer, int id);
        Task DeleteCustomerAsync(int id);
        Task<bool> CheckDuplicateCustomerAsync(string email, string username);
        Task<bool> CheckDuplicateCustomerAsync(string email, string username, int id);  
    }
}
