using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository(BioDbContext context) : BaseRepository<Company>(context), ICompanyRepository
    {
        public async Task<Company> GetByEmailAsync(string email)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Company>> GetCompaniesByApprovalStatusAsync(AccountApprovalStatus status)
        {
            return await _context.Companies.Where(c => c.ApprovalStatus == status).ToListAsync();
        }

        public async Task<Company> GetVknAsync(string vkn)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Vkn == vkn);
        }

        public async Task<Company> GetCompanyWithJobPosts(Guid companyId)
        {
            return await _context.Companies
                .Include(u => u.JobPosts)
                    .ThenInclude(jp => jp.JobApplications)
                .FirstOrDefaultAsync(u => u.Id == companyId);
        }

        public async Task<(IEnumerable<Company>, int)> GetAllCompanyPagedAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Companies.CountAsync();

            var companies = await _context.Companies
                .Include(c => c.JobPosts)
                .OrderBy(j => j.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (companies, totalItems);
        }

    }
}
