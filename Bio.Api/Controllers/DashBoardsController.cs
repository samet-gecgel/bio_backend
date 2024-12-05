using Bio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAdmin")]
    public class DashboardController : CustomBaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Kullanıcı istatistikleri
        [HttpGet("user-statistics")]
        public async Task<IActionResult> GetUserStatistics()
        {
            var result = await _dashboardService.GetUserStatisticsAsync();
            return CreateActionResult(result);
        }

        // Şirket istatistikleri
        [HttpGet("company-statistics")]
        public async Task<IActionResult> GetCompanyStatistics()
        {
            var result = await _dashboardService.GetCompanyStatisticsAsync();
            return CreateActionResult(result);
        }

        // Başvuru istatistikleri
        [HttpGet("application-statistics")]
        public async Task<IActionResult> GetApplicationStatistics()
        {
            var result = await _dashboardService.GetApplicationStatisticsAsync();
            return CreateActionResult(result);
        }

        // İş ilanı istatistikleri
        [HttpGet("jobpost-statistics")]
        public async Task<IActionResult> GetJobPostStatistics()
        {
            var result = await _dashboardService.GetJobPostStatisticsAsync();
            return CreateActionResult(result);
        }

        // Platform genel istatistikleri
        [HttpGet("platform-statistics")]
        public async Task<IActionResult> GetPlatformStatistics()
        {
            var result = await _dashboardService.GetPlatformStatisticsAsync();
            return CreateActionResult(result);
        }

        [HttpGet("job-applications-last-7-days")]
        public async Task<IActionResult> GetJobApplicationsLast7Days()
        {
            var result = await _dashboardService.GetJobApplicationsLast7DaysAsync();
            return CreateActionResult(result);
        }
    }
}
