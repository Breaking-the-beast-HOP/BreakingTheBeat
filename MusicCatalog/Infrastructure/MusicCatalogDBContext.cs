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
        }

    }
}

