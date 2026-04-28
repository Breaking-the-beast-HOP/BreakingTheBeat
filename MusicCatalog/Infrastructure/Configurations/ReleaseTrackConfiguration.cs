using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations
{
    public class ReleaseTrackConfiguration : IEntityTypeConfiguration<ReleaseTrack>
    {
        public void Configure(EntityTypeBuilder<ReleaseTrack> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.TrackNumber)
                .IsRequired();

            builder.HasOne(rt => rt.Track)
                .WithMany()
                .HasForeignKey(rt => rt.TrackId)
                .OnDelete(DeleteBehavior.Restrict); // 🔥 vigtigt
        }
    }
}
