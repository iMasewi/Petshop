using LoginUpLevel.DTOs;
using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface IOrderAdressRepository : IRepository<OrderAdress>
    {
        Task<IEnumerable<OrderAdress>> GetOrderAdressByCustomer(int customerId);
    }
}
