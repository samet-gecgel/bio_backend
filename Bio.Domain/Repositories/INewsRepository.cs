using Bio.Domain.Entities;

namespace Bio.Domain.Repositories
{
    public interface INewsRepository : IBaseRepository<News>
    {
        Task<(IEnumerable<News>, int)> GetAllNewsPagedAsync(int pageNumber, int pageSize);
    }
}
