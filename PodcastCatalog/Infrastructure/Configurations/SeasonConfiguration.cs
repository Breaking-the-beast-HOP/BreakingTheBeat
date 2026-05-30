using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> entity)
        {
            entity.ToTable("Season");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Title)
                .IsRequired(false)
                .HasColumnType("text");

            entity.Property(s => s.SeasonNumber)
                .IsRequired();

            entity.Property(s => s.ReleaseDate)
                .IsRequired(false)
                .HasColumnType("date");

            entity.HasIndex(s => new
            {
                s.PodcastId,
                s.SeasonNumber
            }).IsUnique();

            entity.HasOne(s => s.Podcast)
                .WithMany(p => p.Seasons)
                .HasForeignKey(s => s.PodcastId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Metadata
                .FindNavigation(nameof(Season.Episodes))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}