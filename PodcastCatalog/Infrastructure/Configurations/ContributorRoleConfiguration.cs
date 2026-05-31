using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Configurations
{
    public class ContributorRoleConfiguration : IEntityTypeConfiguration<ContributorRole>
    {
        public void Configure(EntityTypeBuilder<ContributorRole> entity)
        {
            entity.ToTable("ContributorRole");

            entity.HasKey(cr => cr.Id);

            entity.Property(cr => cr.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(cr => cr.Description)
                .IsRequired(false);
        }
    }
}