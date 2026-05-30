using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Configurations
{
    public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> entity)
        {
            entity.ToTable("Episode");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.SeasonId)
                .IsRequired();

            entity.Property(e => e.Title)
                .IsRequired(false)
                .HasMaxLength(250);

            entity.Property(e => e.Description)
                .IsRequired(false);

            entity.Property(e => e.Duration)
                .IsRequired(false);

            entity.Property(e => e.AiringDate)
                .IsRequired(false);

            entity.Property(e => e.EpisodeNumber)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => new
            {
                e.SeasonId,
                e.EpisodeNumber
            }).IsUnique();

            entity.HasOne(e => e.Season)
                .WithMany(s => s.Episodes)
                .HasForeignKey(e => e.SeasonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.EpisodeTag)
                .WithMany(t => t.Episodes)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            entity.Metadata
                .FindNavigation(nameof(Episode.Contributions))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}