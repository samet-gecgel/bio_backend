using Bio.Domain.Entities;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class JobApplicationRepository(BioDbContext context) : BaseRepository<JobApplication>(context), IJobApplicationRepository
    {
        public async Task<IEnumerable<JobApplication>> GetByJobPostIdAsync(Guid jobPostId)
        {
            return await _context.JobApplications.Where(ja => ja.JobPostId == jobPostId).ToListAsync();
        }

        public async Task<List<JobApplication>> GetApplicationsByCompanyIdAsync(Guid companyId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobPost)
                .ThenInclude(jp => jp.Company)
                .Include(ja => ja.User)   
                .Where(ja => ja.JobPost.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<JobApplication> GetApplicationDetailByIdAsync(Guid applicationId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobPost)
                .ThenInclude(jp => jp.Company)
                .Include(ja => ja.Resume)         
                .Include(ja => ja.User)           
                .ThenInclude(u => u.Certificates) 
                .FirstOrDefaultAsync(ja => ja.Id == applicationId);
        }


        public async Task<IEnumerable<JobApplication>> GetByUserIdAsync(Guid userId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobPost)
                .ThenInclude(jp => jp.Company)
                .Where(ja => ja.UserId == userId).ToListAsync();
        }
    }
}
