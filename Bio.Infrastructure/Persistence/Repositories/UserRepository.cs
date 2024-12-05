using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class UserRepository(BioDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<User> GetEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersByApprovalStatusAsync(AccountApprovalStatus status)
        {
            return await _context.Users.Where(u => u.ApprovalStatus == status).ToListAsync();
        }

        public async Task<User> GetUserWithResumesCertificatesAndJobApplication(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Resumes)
                .Include(u => u.Certificates)
                .Include(u => u.JobApplications)
                    .ThenInclude(ja => ja.JobPost) 
                    .ThenInclude(jp => jp.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<(IEnumerable<User>, int)> GetAllUserPagedAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Users.CountAsync();

            var users = await _context.Users
                .OrderBy(j => j.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalItems);
        }

    }
}