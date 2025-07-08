using LoginUpLevel.Data;
using LoginUpLevel.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginUpLevel.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDbContext _context;
        public StatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetTotalCustomers(DateTime fromDate, DateTime toDate)
        {
            return await _context.Customers
                .CountAsync();
        }

        public async Task<int> GetTotalEmployees()
        {
            return await _context.Employees
                .CountAsync();
        }

        public async Task<int> GetTotalOrders(DateTime fromDate, DateTime toDate)
        {
            return await _context.Orders
                .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
                .CountAsync();
        }

        public async Task<int> GetTotalOrdersToDay()
        {
            return await _context.Orders
                .Where(o => o.CreatedAt.Date == DateTime.Today)
                .CountAsync();
        }

        public async Task<float> GetTotalPrice(DateTime fromDate, DateTime toDate)
        {
            var totalPrice =  await _context.Orders
                .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate && o.StatusId == 3)
                .SumAsync(o => o.TotalPrice);
            if(totalPrice == null)
            {
                throw new Exception("Error");
            }

            return float.Parse(totalPrice.ToString());
        }

        public async Task<float> GetTotalPriceToDay()
        {
            var totalPriceToDay = await _context.Orders
                .Where(o => o.CreatedAt.Date == DateTime.Today && o.StatusId == 3)
                .SumAsync(o => o.TotalPrice);
            if (totalPriceToDay == null)
            {
                throw new Exception("Error");
            }
            return float.Parse(totalPriceToDay.ToString());
        }

        public async Task<int> GetTotalProducts()
        {
            return await _context.Products
                .CountAsync();
        }
    }
}
