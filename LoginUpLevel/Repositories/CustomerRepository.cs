using LoginUpLevel.Data;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Task<bool> CheckDuplicateCustomer(string email, string username)
        {
            return _context.Customers
                .AnyAsync(c => (c.UserName == username || c.Email == email));
        }
        public Task<bool> CheckDuplicateCustomer(string email, string username, int id)
        {
            return _context.Customers
                .AnyAsync(c => (c.UserName == username || c.Email == email) && c.Id != id);
        }
    }
}