using Bio.Domain.Entities;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class CertificateRepository(BioDbContext context) : BaseRepository<Certificate>(context), ICertificateRepository
    {
        public async Task<IEnumerable<Certificate>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Certificates.Where(c => c.UserId == userId).ToListAsync();
        }
    }
}
