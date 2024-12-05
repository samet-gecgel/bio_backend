using Bio.Domain.Entities;

namespace Bio.Domain.Repositories
{
    public interface IDashboardCompanyRepository
    {
        Task<int> GetTotalJobPostsByCompanyAsync(Guid companyId);
        Task<int> GetActiveJobPostsByCompanyAsync(Guid companyId);
        Task<int> GetApprovedJobPostsByCompanyAsync(Guid companyId);
        Task<int> GetPendingJobPostsByCompanyAsync(Guid companyId);
        Task<int> GetRejectedJobPostsByCompanyAsync(Guid companyId);
        Task<int> GetTotalApplicationsByCompanyAsync(Guid companyId);
        Task<int> GetTotalJobPostViewsByCompanyAsync(Guid companyId);
        Task<List<JobPost>> GetActiveJobPostsTableByCompanyAsync(Guid companyId);
        Task<List<JobApplication>> GetRecentApplicationsByCompanyAsync(Guid companyId);
        Task<List<KeyValuePair<string, int>>> GetJobApplicationsLast7DaysByCompanyAsync(Guid companyId);
        Task<List<KeyValuePair<string, int>>> GetWeeklyJobPostViewsByCompanyAsync(Guid companyId);
    }
}
