using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Configurations
{
    public class ReleaseConfiguration : IEntityTypeConfiguration<Release>
    {
        public void Configure(EntityTypeBuilder<Release> builder)
        {
            builder.ToTable("Release");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedNever();

            builder.Property(r => r.Title)
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Version)
                .HasColumnName("Version")
                .HasMaxLength(100);

            builder.Property(r => r.EAN)
                .HasColumnName("EAN")
                .HasMaxLength(50);

            builder.Property(r => r.ReleaseDate)
                .HasColumnName("ReleaseDate")
                .HasColumnType("timestamp without time zone");

            builder.HasOne(r => r.Genre)
                .WithMany()
                .HasForeignKey(r => r.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(l => l.LanguageId)
                .HasColumnName("MetadataLanguageId")
                .IsRequired();

            builder.HasOne(r => r.Language)
                .WithMany()
                .HasForeignKey(r => r.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(r => r.Volumes)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(r => r.Volumes)
                .WithOne(v => v.Release)
                .HasForeignKey(v => v.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(r => r.Contributions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
