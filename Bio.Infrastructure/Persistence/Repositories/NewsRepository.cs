using Bio.Domain.Entities;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Repositories
{
    public class NewsRepository(BioDbContext context) : BaseRepository<News>(context), INewsRepository
    {
        public async Task<(IEnumerable<News>, int)> GetAllNewsPagedAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.News.CountAsync();
            var news = await _context.News
                .Include(n => n.Admin)
                .OrderBy(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (news, totalItems);
        }
    }
}
