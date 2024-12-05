using Bio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bio.Infrastructure.Persistence.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title).IsRequired().HasMaxLength(1000);

            builder.Property(n => n.Description).IsRequired();

            builder.Property(n => n.ImagePath).IsRequired();

            builder.HasOne(n => n.Admin)
                .WithMany(a => a.News)
                .HasForeignKey(n => n.AdminId);
        }
    }
}
