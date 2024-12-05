using AutoMapper;
using Bio.Application.Dtos.Dashboard;
using Bio.Application.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using System.Net;

namespace Bio.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMapper _mapper;

        public DashboardService(IDashboardRepository dashboardRepository, IMapper mapper)
        {
            _dashboardRepository = dashboardRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserStatisticsDto>> GetUserStatisticsAsync()
        {
            var result = new UserStatisticsDto
            {
                TotalUsers = await _dashboardRepository.GetTotalUsersAsync(),
                ApprovedUsers = await _dashboardRepository.GetApprovedUsersAsync(),
                NewUsersLast30Days = await _dashboardRepository.GetNewUsersLast30DaysAsync()
            };

            return ServiceResult<UserStatisticsDto>.Success(result);
        }

        public async Task<ServiceResult<CompanyStatisticsDto>> GetCompanyStatisticsAsync()
        {
            var result = new CompanyStatisticsDto
            {
                TotalCompanies = await _dashboardRepository.GetTotalCompaniesAsync(),
                ApprovedCompanies = await _dashboardRepository.GetApprovedCompaniesAsync(),
                NewCompaniesLast30Days = await _dashboardRepository.GetNewCompaniesLast30DaysAsync()
            };

            return ServiceResult<CompanyStatisticsDto>.Success(result);
        }

        public async Task<ServiceResult<ApplicationStatisticsDto>> GetApplicationStatisticsAsync()
        {
            var recentApplications = _mapper.Map<List<RecentApplicationDto>>(await _dashboardRepository.GetRecentApplicationsAsync());

            var mostAppliedCompanies = _mapper.Map<List<MostAppliedCompaniesDto>>(
                await _dashboardRepository.GetTopCompaniesLast30DaysAsync()
            );

            var result = new ApplicationStatisticsDto
            {
                TotalApplications = await _dashboardRepository.GetTotalApplicationsAsync(),
                RecentApplications = recentApplications,
                MostAppliedCompanies = mostAppliedCompanies
            };

            return ServiceResult<ApplicationStatisticsDto>.Success(result);
        }

        public async Task<ServiceResult<JobPostStatisticsDto>> GetJobPostStatisticsAsync()
        {
            var result = new JobPostStatisticsDto
            {
                ActiveJobPosts = await _dashboardRepository.GetActiveJobPostsAsync(),
                TotalJobPosts = await _dashboardRepository.GetTotalJobPostsAsync(),
                NewJobPostsLast30Days = await _dashboardRepository.GetNewJobPostsLast30DaysAsync()
            };

            return ServiceResult<JobPostStatisticsDto>.Success(result);
        }

        public async Task<ServiceResult<PlatformStatisticsDto>> GetPlatformStatisticsAsync()
        {
            var userStats = await GetUserStatisticsAsync();
            var companyStats = await GetCompanyStatisticsAsync();
            var applicationStats = await GetApplicationStatisticsAsync();
            var jobPostStats = await GetJobPostStatisticsAsync();

            var result = new PlatformStatisticsDto
            {
                UserStatistics = userStats.Data,
                CompanyStatistics = companyStats.Data,
                ApplicationStatistics = applicationStats.Data,
                JobPostStatistics = jobPostStats.Data
            };

            return ServiceResult<PlatformStatisticsDto>.Success(result);
        }

        public async Task<ServiceResult<List<KeyValuePair<string, int>>>> GetJobApplicationsLast7DaysAsync()
        {
            var data = await _dashboardRepository.GetJobApplicationsLast7DaysAsync();

            if (data == null || !data.Any())
            {
                return ServiceResult<List<KeyValuePair<string, int>>>.Fail("Son 7 gün için başvuru bulunamadı.", HttpStatusCode.NotFound);
            }

            return ServiceResult<List<KeyValuePair<string, int>>>.Success(data);
        }
    }
}
