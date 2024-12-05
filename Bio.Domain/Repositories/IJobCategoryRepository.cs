using Bio.Domain.Entities;

namespace Bio.Domain.Repositories
{
    public interface IJobCategoryRepository : IBaseRepository<JobCategory>
    {
        Task<JobCategory> GetCategoryByNameAsync(string categoryName);
    }
}
