using LoginUpLevel.Data;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckDuplicateEmployee(string email, string username, int id)
        {
            return await _context.Employees
                .AnyAsync(x => (x.UserName == username || x.Email == email) && x.Id != id);
        }

        public Task<bool> CheckDuplicateEmployee(string email, string username)
        {
            return _context.Employees
                .AnyAsync(x => (x.UserName == username || x.Email == email));
        }
    }
}
