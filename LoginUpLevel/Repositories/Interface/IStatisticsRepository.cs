using LoginUpLevel.Data;
using LoginUpLevel.DTOs;

namespace LoginUpLevel.Repositories.Interface
{
    public interface IStatisticsRepository
    {
        Task<float> GetTotalPrice(DateTime fromDate, DateTime toDate);
        Task<float> GetTotalPriceToDay();
        Task<int> GetTotalOrders(DateTime fromDate, DateTime toDate);
        Task<int> GetTotalOrdersToDay();
        Task<int> GetTotalCustomers(DateTime fromDate, DateTime toDate);
        Task<int> GetTotalEmployees();
        Task<int> GetTotalProducts();
    }
}
