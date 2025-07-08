using LoginUpLevel.DTOs;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;

namespace LoginUpLevel.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StatisticsDTO> GetStatisticsAsync(StatisticsDTO statisticsDto)
        {
            try
            {
                float totalPrice = await _unitOfWork.StatisticsRepository.GetTotalPrice(statisticsDto.FromDate, statisticsDto.ToDate);
                float totalPriceToDay = await _unitOfWork.StatisticsRepository.GetTotalPriceToDay();
                int totalOrders = await _unitOfWork.StatisticsRepository.GetTotalOrders(statisticsDto.FromDate, statisticsDto.ToDate);
                int totalOrdersToDay = await _unitOfWork.StatisticsRepository.GetTotalOrdersToDay();
                int totalCustomers = await _unitOfWork.StatisticsRepository.GetTotalCustomers(statisticsDto.FromDate, statisticsDto.ToDate);
                int totalEmployees = await _unitOfWork.StatisticsRepository.GetTotalEmployees();
                int totalProducts = await _unitOfWork.StatisticsRepository.GetTotalProducts();

                return new StatisticsDTO
                {
                    FromDate = statisticsDto.FromDate,
                    ToDate = statisticsDto.ToDate,
                    TotalPrice = totalPrice,
                    TotalPriceToday = totalPriceToDay,
                    TotalOrders = totalOrders,
                    TotalOrdersToday = totalOrdersToDay,
                    TotalCustomers = totalCustomers,
                    TotalEmployee = totalEmployees,
                    TotalProducts = totalProducts
                };
            } catch (Exception ex)
            {
                throw new Exception("Failed to retrieve statistics", ex);
            }
        }
    }
}
