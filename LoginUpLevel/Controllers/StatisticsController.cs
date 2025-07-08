using LoginUpLevel.DTOs;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginUpLevel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult<StatisticsDTO>> GetStatistics([FromBody] StatisticsDTO statisticsDto)
        {
            try
            {
                var statictics = await _statisticsService.GetStatisticsAsync(statisticsDto);

                if (statictics == null)
                {
                    return NotFound(new { message = "Statistics not found" });
                }

                return Ok(statictics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
