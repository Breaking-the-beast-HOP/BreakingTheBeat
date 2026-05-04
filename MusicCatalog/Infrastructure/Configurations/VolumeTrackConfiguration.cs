using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations
{
    public class VolumeTrackConfiguration : IEntityTypeConfiguration<VolumeTrack>
    {
        public void Configure(EntityTypeBuilder<VolumeTrack> builder)
        {
            builder.ToTable("VolumeTrack");

            builder.HasKey(vt => vt.Id);

            builder.Property(vt => vt.Id)
                .ValueGeneratedNever();

            builder.Property(vt => vt.ReleaseVolumeId)
                .HasColumnName("ReleaseVolumeId")
                .IsRequired();

            builder.Property(vt => vt.TrackId)
                .HasColumnName("TrackId")
                .IsRequired();

            builder.Property(vt => vt.TrackNumber)
                .HasColumnName("TrackNumber")
                .IsRequired();

            builder.HasOne(vt => vt.ReleaseVolume)
                .WithMany(v => v.Tracks)
                .HasForeignKey(vt => vt.ReleaseVolumeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vt => vt.Track)
                .WithMany()
                .HasForeignKey(vt => vt.TrackId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
