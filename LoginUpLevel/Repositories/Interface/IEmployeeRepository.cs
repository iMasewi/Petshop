using LoginUpLevel.Models;

namespace LoginUpLevel.Repositories.Interface
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<bool> CheckDuplicateEmployee(string email, string username, int id);
        Task<bool> CheckDuplicateEmployee(string email, string username);
    }
}
