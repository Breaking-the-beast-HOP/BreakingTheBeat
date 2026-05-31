using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> entity)
        {
            entity.ToTable("Language");

            entity.HasKey(l => l.Id);

            entity.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(l => l.ISO)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}