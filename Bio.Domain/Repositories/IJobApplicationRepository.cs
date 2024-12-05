using Bio.Domain.Entities;

namespace Bio.Domain.Repositories
{
    public interface IJobApplicationRepository : IBaseRepository<JobApplication>
    {
        Task<IEnumerable<JobApplication>> GetByUserIdAsync(Guid userId);
        Task<List<JobApplication>> GetApplicationsByCompanyIdAsync(Guid companyId);
        Task<JobApplication> GetApplicationDetailByIdAsync(Guid applicationId);
        Task<IEnumerable<JobApplication>> GetByJobPostIdAsync(Guid jobPostId);
    }
}
