using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(ja => ja.CoverLetter).IsRequired().HasMaxLength(1000);

            builder.Property(ja => ja.Status).IsRequired().HasConversion<int>();

            builder.HasOne(ja => ja.JobPost)
                .WithMany(jp => jp.JobApplications)
                .HasForeignKey(jp => jp.JobPostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ja => ja.User)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(ja => ja.UserId);

            builder.HasOne(ja => ja.Resume)
                .WithMany()
                .HasForeignKey(ja => ja.ResumeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
