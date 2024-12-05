using Bio.Application.Dtos.Dashboard;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<ServiceResult<UserStatisticsDto>> GetUserStatisticsAsync();
        Task<ServiceResult<CompanyStatisticsDto>> GetCompanyStatisticsAsync();
        Task<ServiceResult<ApplicationStatisticsDto>> GetApplicationStatisticsAsync();
        Task<ServiceResult<JobPostStatisticsDto>> GetJobPostStatisticsAsync();
        Task<ServiceResult<PlatformStatisticsDto>> GetPlatformStatisticsAsync();
        Task<ServiceResult<List<KeyValuePair<string, int>>>> GetJobApplicationsLast7DaysAsync();
    }
}
