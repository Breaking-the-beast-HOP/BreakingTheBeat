using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations
{
    public class ContributorRoleConfiguration: IEntityTypeConfiguration<ContributorRole>
    {
        public void Configure(EntityTypeBuilder<ContributorRole> entity)
        {
            entity.ToTable("ContributorRole");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedNever();

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        } 
    }
}
 
