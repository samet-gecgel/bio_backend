using Bio.Domain.Interfaces;
using Bio.Infrastructure.Persistence.Contexts;
using System;

namespace Bio.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BioDbContext _context;

        public UnitOfWork(BioDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
