using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure
{
    public class MusicCatalogDbContext : DbContext
    {
        public MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options)
            : base(options)
        {
        }
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Track> Tracks => Set<Track>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.ISO)
                    .HasColumnName("ISO")
                    .HasMaxLength(10)
                    .IsRequired();
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            modelBuilder.Entity<Track>(entity =>
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

                entity.Property(x => x.Isrc)
                    .HasColumnName("ISRC")
                    .HasMaxLength(50);

                entity.Property(x => x.Duration)
                     .HasColumnName("DurationSeconds")
                     .HasConversion(
                         v => (int)v.TotalSeconds,
                         v => TimeSpan.FromSeconds(v))
                     .IsRequired();

                entity.Property(x => x.GenreId)
                    .IsRequired();

                entity.Property(x => x.MetadataLanguageId)
                    .IsRequired();

                entity.Property(x => x.AudioLanguageId)
                    .IsRequired();

                entity.HasOne(x => x.Genre)
                    .WithMany()
                    .HasForeignKey(x => x.GenreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.MetadataLanguage)
                    .WithMany()
                    .HasForeignKey(x => x.MetadataLanguageId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AudioLanguage)
                    .WithMany()
                    .HasForeignKey(x => x.AudioLanguageId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Ignore(x => x.Contributions);
            });
        }
    }
}