using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure
{
    public class MusicCatalogDbContext : DbContext
    {
        public MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Track> Tracks => Set<Track>();
        public DbSet<Release> Releases => Set<Release>();
        public DbSet<Volume> Volumes => Set<Volume>();
        public DbSet<Contribution> Contributions => Set<Contribution>();
        public DbSet<Contributor> Contributors => Set<Contributor>();
        public DbSet<ContributorRole> ContributorRoles => Set<ContributorRole>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Language> Languages => Set<Language>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.ISO)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Release>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Volume>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            //modelBuilder.Entity<Contribution>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //});

            modelBuilder.Entity<Contributor>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ContributorRole>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}

