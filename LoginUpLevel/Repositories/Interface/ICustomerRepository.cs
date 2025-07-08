using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<bool> CheckDuplicateCustomer(string email, string username);
        Task<bool> CheckDuplicateCustomer(string email, string username, int id);
    }
}
