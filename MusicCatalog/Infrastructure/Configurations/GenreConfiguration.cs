using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;
namespace MusicCatalog.Infrastructure.Configurations 
{ 
    public class GenreConfiguration : IEntityTypeConfiguration<Genre> 
    { 
        public void Configure(EntityTypeBuilder<Genre> entity) 
        { 
            entity.ToTable("Genre"); 

            entity.HasKey(x => x.Id); 

            entity.Property(x => x.Id)
                .ValueGeneratedNever(); 

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired(); 
        } 
    }
}