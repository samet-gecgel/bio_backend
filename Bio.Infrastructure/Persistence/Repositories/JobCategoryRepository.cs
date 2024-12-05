using Bio.Domain.Entities;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class JobCategoryRepository(BioDbContext context) : BaseRepository<JobCategory>(context), IJobCategoryRepository
    {
        public async Task<JobCategory> GetCategoryByNameAsync(string categoryName)
        {
            return await _context.JobCategories.FirstOrDefaultAsync(jc => jc.Name == categoryName);
        }
    }
}
