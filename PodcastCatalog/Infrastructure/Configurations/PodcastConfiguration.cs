using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Configurations
{
    public class PodcastConfiguration : IEntityTypeConfiguration<Podcast>
    {
        public void Configure(EntityTypeBuilder<Podcast> entity)
        {
            entity.ToTable("Podcast");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .ValueGeneratedNever();

            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(p => p.Description)
                .IsRequired(false);

            entity.Property(p => p.CategoryId)
                .IsRequired();

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Podcasts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(p => p.LanguageId)
                .IsRequired();

            entity.HasOne(p => p.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(p => p.Contributions)
                .WithOne(c => c.Podcast)
                .HasForeignKey(c => c.PodcastId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Navigation(p => p.Contributions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata
                .FindNavigation(nameof(Podcast.Seasons))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}