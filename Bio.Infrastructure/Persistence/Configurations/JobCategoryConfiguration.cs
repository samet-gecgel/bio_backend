using Bio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class JobCategoryConfiguration : IEntityTypeConfiguration<JobCategory>
    {
        public void Configure(EntityTypeBuilder<JobCategory> builder)
        {
            builder.HasKey(jc => jc.Id);

            builder.Property(jc => jc.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(jc => jc.JobPosts)
                .WithOne(jp => jp.JobCategory)
                .HasForeignKey(jc => jc.CategoryId);
        }
    }
}
