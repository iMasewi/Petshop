using LoginUpLevel.Data;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class OrderAdressRepository : Repository<OrderAdress>, IOrderAdressRepository
    {
        public OrderAdressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderAdress>> GetOrderAdressByCustomer(int customerId)
        {
            try
            {
                return await _context.OrderAdress.Where(u => u.CustomerId == customerId).ToListAsync();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
