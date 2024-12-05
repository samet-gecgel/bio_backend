using Bio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bio.Infrastructure.Persistence.Contexts
{
    public class BioDbContext(DbContextOptions options) : DbContext(options)
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BioDbContext).Assembly);
        }
    }
}
