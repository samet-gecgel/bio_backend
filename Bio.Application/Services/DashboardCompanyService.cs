using AutoMapper;
using Bio.Application.Dtos.Dashboard;
using Bio.Application.Dtos.JobPost;
using Bio.Application.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bio.Application.Services
{
    public class DashboardCompanyService : IDashboardCompanyService
    {
        private readonly IDashboardCompanyRepository _dashboardCompanyRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardCompanyService(IDashboardCompanyRepository dashboardCompanyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardCompanyRepository = dashboardCompanyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCompanyIdFromToken()
        {
            var companyIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (companyIdClaim == null || !Guid.TryParse(companyIdClaim.Value, out var companyId))
            {
                throw new UnauthorizedAccessException("Şirket kimliği doğrulanamadı veya geçersiz.");
            }

            return companyId;
        }

        public async Task<ServiceResult<CompanyDashboardDto>> GetCompanyDashboardAsync()
        {
            var companyId = GetCompanyIdFromToken();

            // Firma ID'ye göre tüm istatistikleri al
            var totalJobPosts = await _dashboardCompanyRepository.GetTotalJobPostsByCompanyAsync(companyId);
            var activeJobPosts = await _dashboardCompanyRepository.GetActiveJobPostsByCompanyAsync(companyId);
            var approvedJobPosts = await _dashboardCompanyRepository.GetApprovedJobPostsByCompanyAsync(companyId);
            var pendingJobPosts = await _dashboardCompanyRepository.GetPendingJobPostsByCompanyAsync(companyId);
            var rejectedJobPosts = await _dashboardCompanyRepository.GetRejectedJobPostsByCompanyAsync(companyId);
            var totalApplications = await _dashboardCompanyRepository.GetTotalApplicationsByCompanyAsync(companyId);
            var totalJobPostViews = await _dashboardCompanyRepository.GetTotalJobPostViewsByCompanyAsync(companyId);

            // Aktif ilanlar ve son başvuruları tablo olarak al
            var activeJobPostsTable = _mapper.Map<List<JobPostDto>>(await _dashboardCompanyRepository.GetActiveJobPostsTableByCompanyAsync(companyId));
            var recentApplications = _mapper.Map<List<RecentApplicationDto>>(await _dashboardCompanyRepository.GetRecentApplicationsByCompanyAsync(companyId));

            // Son 7 günlük başvuru ve ilan görüntüleme sayısı
            var jobApplicationsLast7Days = await _dashboardCompanyRepository.GetJobApplicationsLast7DaysByCompanyAsync(companyId);
            //var weeklyJobPostViews = await _dashboardCompanyRepository.GetWeeklyJobPostViewsByCompanyAsync(companyId);

            // DTO oluştur
            var dashboardData = new CompanyDashboardDto
            {
                TotalJobPosts = totalJobPosts,
                ActiveJobPosts = activeJobPosts,
                ApprovedJobPosts = approvedJobPosts,
                PendingJobPosts = pendingJobPosts,
                RejectedJobPosts = rejectedJobPosts,
                TotalApplications = totalApplications,
                TotalJobPostViews = totalJobPostViews,
                ActiveJobPostsTable = activeJobPostsTable,
                RecentApplications = recentApplications,
                JobApplicationsLast7Days = jobApplicationsLast7Days,
                //WeeklyJobPostViews = weeklyJobPostViews
            };

            return ServiceResult<CompanyDashboardDto>.Success(dashboardData);
        }
    }
}
