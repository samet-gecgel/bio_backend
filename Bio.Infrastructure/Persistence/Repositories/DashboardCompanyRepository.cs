using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class DashboardCompanyRepository : IDashboardCompanyRepository
    {
        private readonly BioDbContext _context;

        public DashboardCompanyRepository(BioDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetTotalJobPostsByCompanyAsync(Guid companyId)
        {
            return await _context.JobPosts.CountAsync(jp => jp.CompanyId == companyId);
        }

        // Aktif ilan (firmanın)
        public async Task<int> GetActiveJobPostsByCompanyAsync(Guid companyId)
        {
            return await _context.JobPosts.CountAsync(jp => jp.CompanyId == companyId && jp.IsActive);
        }

        // Onaylanan olan ilan
        public async Task<int> GetApprovedJobPostsByCompanyAsync(Guid companyId)
        {
            return await _context.JobPosts.CountAsync(jp => jp.CompanyId == companyId && jp.ApplicationStatus == ApplicationStatus.Approved);
        }

        // Beklemede olan ilan
        public async Task<int> GetPendingJobPostsByCompanyAsync(Guid companyId)
        {
            return await _context.JobPosts.CountAsync(jp => jp.CompanyId == companyId && jp.ApplicationStatus == ApplicationStatus.Pending);
        }

        // Beklemede olan ilan
        public async Task<int> GetRejectedJobPostsByCompanyAsync(Guid companyId)
        {
            return await _context.JobPosts.CountAsync(jp => jp.CompanyId == companyId && jp.ApplicationStatus == ApplicationStatus.Rejected);
        }

        // Toplam başvuru (firmanın)
        public async Task<int> GetTotalApplicationsByCompanyAsync(Guid companyId)
        {
            return await _context.JobApplications.CountAsync(ja => ja.JobPost.CompanyId == companyId);
        }

        // Toplam ilan görüntüleme sayısı (firmanın)
        public async Task<int> GetTotalJobPostViewsByCompanyAsync(Guid companyId)
        {
            return await _context.JobPosts
                .Where(jp => jp.CompanyId == companyId)
                .SumAsync(jp => jp.ViewCount);
        }

        // Aktif iş ilanları table (firmanın)
        public async Task<List<JobPost>> GetActiveJobPostsTableByCompanyAsync(Guid companyId)
        {
            return await _context.JobPosts
                .Where(jp => jp.CompanyId == companyId && jp.IsActive)
                .ToListAsync();
        }

        // Son başvurular (firmanın)
        public async Task<List<JobApplication>> GetRecentApplicationsByCompanyAsync(Guid companyId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobPost).ThenInclude(ja => ja.Company)
                .Include(ja =>ja.User)
                .Where(ja => ja.JobPost.CompanyId == companyId)
                .OrderByDescending(ja => ja.ApplicationDate)
                .Take(10)
                .ToListAsync();
        }

        // Son 7 günün başvuru sayısı (firmanın)
        public async Task<List<KeyValuePair<string, int>>> GetJobApplicationsLast7DaysByCompanyAsync(Guid companyId)
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-6).Date;
            var today = DateTime.UtcNow.Date.AddDays(1);

            var applicationsByDate = await _context.JobApplications
                .Where(ja => ja.ApplicationDate >= sevenDaysAgo && ja.ApplicationDate < today && ja.JobPost.CompanyId == companyId)
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

        // Haftalık ilan görüntüleme sayısı (firmanın)
        public async Task<List<KeyValuePair<string, int>>> GetWeeklyJobPostViewsByCompanyAsync(Guid companyId)
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-6).Date;
            var today = DateTime.UtcNow.Date.AddDays(1);

            var viewsByDate = await _context.JobPosts
                .Where(jp => jp.CompanyId == companyId && jp.ApplicationDeadline >= sevenDaysAgo && jp.ApplicationDeadline < today)
                .GroupBy(jp => jp.ApplicationDeadline.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    TotalViews = group.Sum(jp => jp.ViewCount)
                })
                .ToListAsync();

            var result = Enumerable.Range(0, 7)
                .Select(offset => sevenDaysAgo.AddDays(offset).Date)
                .Select(date => new KeyValuePair<string, int>(
                    date.ToString("d MMM"),
                    viewsByDate.FirstOrDefault(a => a.Date == date)?.TotalViews ?? 0
                ))
                .ToList();

            return result;
        }
    }
}
