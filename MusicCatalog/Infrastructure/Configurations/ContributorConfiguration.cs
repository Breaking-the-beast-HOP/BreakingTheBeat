using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations 
{ 
    public class ContributorConfiguration : IEntityTypeConfiguration<Contributor> 
    { 
        public void Configure(EntityTypeBuilder<Contributor> entity) 
        { 
            entity.ToTable("Contributor"); 

            entity.HasKey(x => x.Id); 

            entity.Property(x => x.LegalName)
                .HasMaxLength(100).IsRequired();
            
            entity.Property(x => x.DisplayName)
                .HasMaxLength(100)
                .IsRequired(); 
            
            entity.Property(x => x.IPI)
                .HasColumnName("IPI"); 

            entity.Property(x => x.ISNI)
                .HasColumnName("ISNI"); 
        } 
    } 
}
