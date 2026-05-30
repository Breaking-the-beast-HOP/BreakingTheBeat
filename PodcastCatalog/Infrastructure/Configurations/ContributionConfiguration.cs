using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Configurations
{
    public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
    {
        public void Configure(EntityTypeBuilder<Contribution> entity)
        {
            entity.ToTable("Contribution");

            entity.HasKey(c => c.Id);

            entity.HasOne(c => c.Contributor)
                .WithMany()
                .HasForeignKey(c => c.ContributorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Podcast)
                .WithMany(p => p.Contributions)
                .HasForeignKey(c => c.PodcastId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            entity.HasOne(c => c.Episode)
                .WithMany(e => e.Contributions)
                .HasForeignKey(c => c.EpisodeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            entity.HasOne(c => c.Role)
                .WithMany()
                .HasForeignKey(c => c.ContributorRoleId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.HasRights)
                .IsRequired();
        }
    }
}