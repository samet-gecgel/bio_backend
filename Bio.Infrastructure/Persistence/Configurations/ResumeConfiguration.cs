using Bio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class ResumeConfiguration : IEntityTypeConfiguration<Resume>
    {
        public void Configure(EntityTypeBuilder<Resume> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ResumeName).HasMaxLength(500);

            builder.Property(r => r.Summary).HasMaxLength(1000);

            builder.Property(r => r.Education).HasMaxLength(1000);

            builder.Property(r => r.Experience).HasMaxLength(1000);

            builder.Property(r => r.Skills).HasMaxLength(500);

            builder.Property(r => r.Languages).HasMaxLength(500);

            builder.Property(r => r.Hobbies).HasMaxLength(500);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Resumes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Resumes)
                .HasForeignKey(r => r.UserId);
        }
    }
}
