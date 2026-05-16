using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicQualityControl.Domain.ReadModels;

namespace MusicQualityControl.Infrastructure.Configurations;

public class ReleaseQcQueueItemConfiguration : IEntityTypeConfiguration<ReleaseQcQueueItem>
{
    public void Configure(EntityTypeBuilder<ReleaseQcQueueItem> builder)
    {
        builder.ToTable("release_qc_queue_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired();
    }
}