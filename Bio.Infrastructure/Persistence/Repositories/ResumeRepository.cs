using Bio.Domain.Entities;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class ResumeRepository(BioDbContext context) : BaseRepository<Resume>(context), IResumeRepository
    {
        public async Task<IEnumerable<Resume>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Resumes
                .Where(r => r.UserId == userId).ToListAsync();
        }
    }
}
