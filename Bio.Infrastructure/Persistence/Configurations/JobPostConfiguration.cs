using Bio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class JobPostConfiguration : IEntityTypeConfiguration<JobPost>
    {
        public void Configure(EntityTypeBuilder<JobPost> builder)
        {
            builder.HasKey(jp => jp.Id);

            builder.Property(jp => jp.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(jp => jp.Description)
                   .IsRequired();

            builder.Property(jp => jp.MinSalary)
                   .HasPrecision(18, 2);

            builder.Property(jp => jp.MaxSalary)
                   .HasPrecision(18, 2);

            builder.Property(jp => jp.District)
                   .HasMaxLength(100);

            builder.Property(jp => jp.Benefits)
                   .HasMaxLength(500);

            builder.Property(jp => jp.OffDays)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(jp => jp.MinAge)
                   .HasMaxLength(3);

            builder.Property(jp => jp.MaxAge)
                   .HasMaxLength(3);

            builder.HasOne(jp => jp.Company)
                   .WithMany(c => c.JobPosts)
                   .HasForeignKey(jp => jp.CompanyId);

            builder.HasOne(jp => jp.JobCategory)
                   .WithMany(jc => jc.JobPosts)
                   .HasForeignKey(jp => jp.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Property(jp => jp.JobType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(jp => jp.RequiredEducationLevel)
                    .IsRequired()
                    .HasConversion<int>();

            builder.Property(jp => jp.ExperienceLevel)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(jp => jp.IsDisabledFriendly)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(c => c.ApplicationStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(jp => jp.ViewCount)
                .HasDefaultValue(0);

            builder.HasMany(jp => jp.JobApplications)
                   .WithOne(ja => ja.JobPost)
                   .HasForeignKey(ja => ja.JobPostId);

            builder.Property(jp => jp.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
