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

            entity.Property(c => c.Id)
                .ValueGeneratedNever();

            entity.Property(c => c.ContributorId)
                .IsRequired();

            entity.HasOne(c => c.Contributor)
                .WithMany()
                .HasForeignKey(c => c.ContributorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(c => c.ContributorRoleId)
                .IsRequired();

            entity.HasOne(c => c.Role)
                .WithMany()
                .HasForeignKey(c => c.ContributorRoleId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.PodcastId)
                .IsRequired(false);

            entity.HasOne(c => c.Podcast)
                .WithMany(p => p.Contributions)
                .HasForeignKey(c => c.PodcastId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(c => new
            {
                c.PodcastId,
                c.ContributorId,
                c.ContributorRoleId
            }).IsUnique();
        }
    }
}