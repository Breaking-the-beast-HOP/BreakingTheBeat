using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Configurations
{
    public class ContributorConfiguration : IEntityTypeConfiguration<Contributor>
    {
        public void Configure(EntityTypeBuilder<Contributor> entity)
        {
            entity.ToTable("Contributor");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.DisplayName)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(c => c.FullName)
                .IsRequired(false)
                .HasMaxLength(250);

            entity.Property(c => c.Title)
                .IsRequired(false)
                .HasMaxLength(150);

            entity.Property(c => c.Biography)
                .IsRequired(false);

            entity.Property(c => c.Expertise)
                .IsRequired(false)
                .HasMaxLength(500);

            entity.Property(c => c.ProfileUrl)
                .IsRequired(false)
                .HasMaxLength(500);
        }
    }
}