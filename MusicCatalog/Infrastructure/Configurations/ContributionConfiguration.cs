using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations
{
    public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
    {
        public void Configure(EntityTypeBuilder<Contribution> entity)
        {
            entity.ToTable("Contribution");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            // =========================
            // CONTRIBUTOR RELATION
            // =========================
            entity.Property(x => x.ContributorId)
                .HasColumnName("ContributorId")
                .IsRequired();

            entity.HasOne(x => x.Contributor)
                .WithMany()
                .HasForeignKey(x => x.ContributorId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // CONTRIBUTOR ROLE RELATION
            // =========================
            entity.Property(x => x.ContributorRoleId)
                .HasColumnName("ContributorRoleId")
                .IsRequired();

            entity.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.ContributorRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // TRACK RELATION
            // =========================
            entity.Property(x => x.TrackId)
                .HasColumnName("TrackId")
                .IsRequired(false);

            entity.HasOne(x => x.Track)
                .WithMany(x => x.Contributions)
                .HasForeignKey(x => x.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.TrackId,
                x.ContributorId,
                x.ContributorRoleId
            }).IsUnique();

            // =========================
            // RELEASE RELATION
            // =========================
            entity.Property(x => x.ReleaseId)
                .HasColumnName("ReleaseId")
                .IsRequired(false);

            entity.HasOne(x => x.Release)
                .WithMany(x => x.Contributions)
                .HasForeignKey(x => x.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.ReleaseId,
                x.ContributorId,
                x.ContributorRoleId
            }).IsUnique();
        }
    }
}
