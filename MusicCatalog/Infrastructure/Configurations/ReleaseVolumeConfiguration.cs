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

            builder.Property(v => v.Id)
                .ValueGeneratedNever();

            builder.Property(v => v.VolumeNumber)
                .IsRequired();

            builder.Navigation(v => v.Tracks)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(v => v.Tracks)
                .WithOne(rt => rt.ReleaseVolume)
                .HasForeignKey(rt => rt.ReleaseVolumeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
