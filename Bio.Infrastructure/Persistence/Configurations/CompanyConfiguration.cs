using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName).IsRequired().HasMaxLength(150);

            builder.Property(c => c.Vkn).IsRequired().HasMaxLength(11);

            builder.Property(c => c.TcKimlik).IsRequired().HasMaxLength(11);

            builder.Property(c => c.Position).IsRequired().HasMaxLength(100);

            builder.Property(c => c.Email).IsRequired().HasMaxLength(100);

            builder.Property(c => c.Phone).IsRequired().HasMaxLength(15);

            builder.Property(c => c.CompanyName).IsRequired().HasMaxLength(200);

            builder.Property(c => c.Position).IsRequired().HasMaxLength(100);

            builder.Property(c => c.District).IsRequired().HasMaxLength(100);

            builder.Property(c => c.EmployeesInCity).IsRequired().HasConversion<int>();

            builder.Property(c => c.EmployeesInCountry).IsRequired().HasConversion<int>();

            builder.Property(c => c.ApprovalStatus).IsRequired().HasConversion<int>();

            builder.Property(c => c.PasswordHash).IsRequired();

            builder.Property(c => c.PasswordSalt).IsRequired();

            builder.Property(c => c.Role).IsRequired().HasConversion<int>();

            builder.HasMany(c => c.JobPosts)
                .WithOne(jp => jp.Company)
                .HasForeignKey(jp => jp.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
