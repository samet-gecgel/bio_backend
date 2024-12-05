using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class AdminRepository(BioDbContext context) : BaseRepository<Admin>(context), IAdminRepository
    {
        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        }



        public async Task<(IEnumerable<Admin>, int)> GetAllAdminPagedAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Admins
                .Where(a => a.Role == UserRole.Admin)
                .CountAsync();

            var admins = await _context.Admins
                .Where(a => a.Role == UserRole.Admin) 
                .OrderBy(j => j.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (admins, totalItems);
        }

    }
}
