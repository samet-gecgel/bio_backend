using Bio.Domain.Entities;

namespace Bio.Domain.Repositories
{
    public interface ICertificateRepository : IBaseRepository<Certificate>
    {
        Task<IEnumerable<Certificate>> GetByUserIdAsync(Guid userId);
    }
}
