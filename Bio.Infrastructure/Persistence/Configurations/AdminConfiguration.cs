using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FullName).IsRequired().HasMaxLength(100);

            builder.Property(a => a.Email).IsRequired().HasMaxLength(100);

            builder.Property(a => a.Department).IsRequired().HasMaxLength(100);

            builder.Property(a => a.PasswordHash).IsRequired();

            builder.Property(a => a.PasswordSalt).IsRequired();

            builder.Property(a => a.Role).IsRequired().HasConversion<int>();

            builder.HasMany(a => a.News)
                .WithOne(n => n.Admin)
                .HasForeignKey(n => n.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
