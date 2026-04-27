using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;
namespace MusicCatalog.Infrastructure.Configurations 
{ 
    public class TrackConfiguration : IEntityTypeConfiguration<Track> 
    { 
        public void Configure(EntityTypeBuilder<Track> entity) 
        { 
            entity.ToTable("Track");

            entity.HasKey(x => x.Id);
            
            entity.Property(x => x.Id)
                .ValueGeneratedNever(); 

            entity.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired(); 

            entity.Property(x => x.Version)
                .HasMaxLength(200); 

            entity.Property(x => x.ISRC)
                .HasColumnName("ISRC")
                .HasMaxLength(50); 

            entity.Property(x => x.Duration)
                .HasColumnName("DurationSeconds")
                .HasConversion(
                    v => (int)v.TotalSeconds, 
                    v => TimeSpan
                    .FromSeconds(v))
                .IsRequired(); 

            //Genre
            entity.Property(x => x.GenreId)
                .IsRequired();

            entity.HasOne(x => x.Genre)
               .WithMany()
               .HasForeignKey(x => x.GenreId)
               .OnDelete(DeleteBehavior.Restrict);

            //MetaLanguage
            entity.Property(x => x.MetadataLanguageId)
                .IsRequired();

            entity.HasOne(x => x.MetadataLanguage)
                .WithMany()
                .HasForeignKey(x => x.MetadataLanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            //AudioLanguage
            entity.Property(x => x.AudioLanguageId)
                .IsRequired();

            entity.HasOne(x => x.AudioLanguage)
                .WithMany()
                .HasForeignKey(x => x.AudioLanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            //Contributions
            entity.HasMany(x => x.Contributions)
                .WithOne(x => x.Track)
                .HasForeignKey(x => x.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Navigation(x => x.Contributions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);


        } 
    } 
}