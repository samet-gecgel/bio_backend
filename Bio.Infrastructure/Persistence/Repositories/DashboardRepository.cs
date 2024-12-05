using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly BioDbContext _context;

        public DashboardRepository(BioDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalUsersAsync() =>
            await _context.Users.CountAsync();

        public async Task<int> GetApprovedUsersAsync() =>
            await _context.Users.CountAsync(u => u.ApprovalStatus == AccountApprovalStatus.Approved);

        public async Task<int> GetNewUsersLast30DaysAsync() =>
            await _context.Users.CountAsync(u => u.CreatedAt >= DateTime.Now.AddDays(-30));

        public async Task<int> GetTotalCompaniesAsync() =>
            await _context.Companies.CountAsync();

        public async Task<int> GetApprovedCompaniesAsync() =>
            await _context.Companies.CountAsync(c => c.ApprovalStatus == AccountApprovalStatus.Approved);

        public async Task<int> GetNewCompaniesLast30DaysAsync() =>
            await _context.Companies.CountAsync(c => c.CreatedAt >= DateTime.Now.AddDays(-30));

        public async Task<int> GetTotalApplicationsAsync() =>
            await _context.JobApplications.CountAsync();

        public async Task<List<JobApplication>> GetRecentApplicationsAsync() =>
            await _context.JobApplications
                .Include(ja => ja.JobPost).ThenInclude(jp => jp.Company)
                .Include(ja => ja.User)
                .OrderByDescending(ja => ja.ApplicationDate)
                .Take(5)
                .ToListAsync();

        public async Task<List<IGrouping<string, JobApplication>>> GetTopCompaniesLast30DaysAsync() =>
            await _context.JobApplications
                .Include(ja => ja.JobPost).ThenInclude(jp => jp.Company)
                .Where(ja => ja.ApplicationDate >= DateTime.Now.AddDays(-30))
                .GroupBy(ja => ja.JobPost.Company.CompanyName)
                .ToListAsync();

        public async Task<int> GetTotalJobPostsAsync()
        {
            return await _context.JobPosts.CountAsync();
        }

        public async Task<int> GetActiveJobPostsAsync()
        {
            return await _context.JobPosts.CountAsync(j => j.IsActive);
        }

        public async Task<int> GetNewJobPostsLast30DaysAsync()
        {
            return await _context.JobPosts.CountAsync(j => j.PublishedDate >= DateTime.UtcNow.AddDays(-30));
        }

        // son 7 iş ilan başvuru sayısı
        public async Task<List<KeyValuePair<string, int>>> GetJobApplicationsLast7DaysAsync()
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-6).Date; 
            var today = DateTime.UtcNow.Date.AddDays(1); 

            var applicationsByDate = await _context.JobApplications
                .Where(ja => ja.ApplicationDate >= sevenDaysAgo && ja.ApplicationDate < today)
                .GroupBy(ja => ja.ApplicationDate.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    Total = group.Count()
                })
                .ToListAsync();

            var result = Enumerable.Range(0, 7)
                .Select(offset => sevenDaysAgo.AddDays(offset).Date)
                .Select(date => new KeyValuePair<string, int>(
                    date.ToString("d MMM"),
                    applicationsByDate.FirstOrDefault(a => a.Date == date)?.Total ?? 0
                ))
                .ToList();

            return result;
        }



    }

}
