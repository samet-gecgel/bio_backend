using Bio.Domain.Entities;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class JobPostRepository(BioDbContext context) : BaseRepository<JobPost>(context), IJobPostRepository
    {
        public async Task<IEnumerable<JobPost>> GetActiveJobPostAsync()
        {
            return await _context.JobPosts
                .Where(j => j.IsActive) 
                .ToListAsync();
        }

        public async Task<IEnumerable<JobPost>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context.JobPosts.Where(jp => jp.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<JobPost>> GetByCompanyIdAsync(Guid companyId)
        {
            return await _context.JobPosts.Where(jp => jp.CompanyId == companyId).ToListAsync();
        }

        public async Task<(IEnumerable<JobPost>, int)> GetByCompanyIdPagedAsync(Guid companyId, int pageNumber, int pageSize)
        {
            var totalItems = await _context.JobPosts.Where(jp => jp.CompanyId == companyId).CountAsync();

            var jobPosts = await _context.JobPosts
                .Include(jp => jp.Company)
                .Where(jp => jp.CompanyId == companyId)
                .OrderBy(jp => jp.PublishedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (jobPosts, totalItems);
        }

        public async Task<JobPost> GetJobPostWithApplicationsAsync(Guid id)
        {
            return await _context.JobPosts
                .Include(j => j.JobApplications)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<(IEnumerable<JobPost>, int)> GetAllJobPostPagedAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.JobPosts.CountAsync();
            var jobPosts = await _context.JobPosts
                .Include(j => j.JobCategory)
                .Include(j => j.Company)
                .OrderBy(j => j.PublishedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (jobPosts, totalItems);
        }

        public async Task<List<JobPost>> GetLatestJobPostsAsync(int count = 4)
        {
            return await _context.JobPosts
                .Include(j => j.JobCategory)
                .Where(j => j.IsActive)
                .OrderByDescending(j => j.PublishedDate) 
                .Take(count)
                .ToListAsync();
        }
        public async Task<int> CountAsync(Expression<Func<JobPost, bool>> predicate = null)
        {
            return predicate == null
                ? await _context.JobPosts.CountAsync()
                : await _context.JobPosts.CountAsync(predicate);
        }

        public async Task<List<JobPost>> GetPagedJobPostsAsync(int pageNumber, int pageSize, Expression<Func<JobPost, bool>> predicate = null)
        {
            var query = _context.JobPosts
                .Include(jp => jp.Company)
                .Include(jp => jp.JobCategory)
                .AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


    }
}
