using Bio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CertificateName).IsRequired().HasMaxLength(500);

            builder.Property(c => c.Institution).IsRequired().HasMaxLength(100);

            builder.Property(c => c.IssueDate).IsRequired();

            builder.Property(c => c.FilePath).IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(u => u.Certificates)
                .HasForeignKey(u => u.UserId);
        }
    }
}
