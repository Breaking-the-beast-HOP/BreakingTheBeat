using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations
{
    public class ReleaseVolumeConfiguration : IEntityTypeConfiguration<ReleaseVolume>
    {
        public void Configure(EntityTypeBuilder<ReleaseVolume> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.VolumeNumber)
                .IsRequired();

            builder.HasMany(typeof(ReleaseTrack), "_tracks")
                .WithOne(nameof(ReleaseTrack.ReleaseVolume))
                .HasForeignKey(nameof(ReleaseTrack.ReleaseVolumeId))
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
