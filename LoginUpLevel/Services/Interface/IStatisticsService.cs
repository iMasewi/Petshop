using LoginUpLevel.DTOs;

namespace LoginUpLevel.Services.Interface
{
    public interface IStatisticsService
    {
        Task<StatisticsDTO> GetStatisticsAsync(StatisticsDTO statisticsDto);
    }
}
