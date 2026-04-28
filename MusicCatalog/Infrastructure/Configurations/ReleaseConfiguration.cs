using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations
{


    public class ReleaseConfiguration : IEntityTypeConfiguration<Release>
    {
        public void Configure(EntityTypeBuilder<Release> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Version)
                .HasMaxLength(100);

            builder.Property(r => r.EAN)
                .HasMaxLength(50);

            builder.Property(r => r.ReleaseDate);

            builder.HasOne(r => r.Genre)
                .WithMany()
                .HasForeignKey(r => r.GenreId);

            builder.HasOne(r => r.Language)
                .WithMany()
                .HasForeignKey(r => r.LanguageId);

            // 🔥 Volumes (owned-ish style)
            builder.HasMany(typeof(ReleaseVolume), "_volumes")
                .WithOne(nameof(ReleaseVolume.Release))
                .HasForeignKey(nameof(ReleaseVolume.ReleaseId))
                .OnDelete(DeleteBehavior.Cascade);

            // 🔥 Contributions
            builder.HasMany(typeof(Contribution), "_contributions")
                .WithOne(nameof(Contribution.Release))
                .HasForeignKey(nameof(Contribution.ReleaseId))
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
