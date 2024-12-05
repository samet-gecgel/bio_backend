using Bio.Domain.Entities;
using System.Linq.Expressions;

namespace Bio.Domain.Repositories
{
    public interface IJobPostRepository : IBaseRepository<JobPost>
    {
        Task<IEnumerable<JobPost>> GetActiveJobPostAsync();
        Task<IEnumerable<JobPost>> GetByCompanyIdAsync(Guid companyId);
        Task<(IEnumerable<JobPost>, int)> GetByCompanyIdPagedAsync(Guid companyId, int pageNumber, int pageSize);
        Task<IEnumerable<JobPost>> GetByCategoryIdAsync(Guid categoryId);
        Task<JobPost> GetJobPostWithApplicationsAsync(Guid id);
        Task<(IEnumerable<JobPost>, int)> GetAllJobPostPagedAsync(int pageNumber, int pageSize);
        Task<List<JobPost>> GetLatestJobPostsAsync(int count = 4);
        Task<int> CountAsync(Expression<Func<JobPost, bool>> predicate = null);
        Task<List<JobPost>> GetPagedJobPostsAsync(int pageNumber, int pageSize, Expression<Func<JobPost, bool>> predicate = null);
    }
}
