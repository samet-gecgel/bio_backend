using Bio.Domain.Entities;

namespace Bio.Domain.Repositories
{
    public interface IResumeRepository : IBaseRepository<Resume>
    {
        Task<IEnumerable<Resume>> GetByUserIdAsync(Guid userId);
    }
}
