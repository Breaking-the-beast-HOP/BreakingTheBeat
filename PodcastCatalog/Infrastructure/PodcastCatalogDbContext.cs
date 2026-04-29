using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure.Data
{
    public class PodcastCatalogDbContext : DbContext
    {
        public PodcastCatalogDbContext(DbContextOptions<PodcastCatalogDbContext> options)
            : base(options)
        {
        }

        // DbSets for alle entiteter
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<ContributorRole> ContributorRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguration af Podcast
            modelBuilder.Entity<Podcast>(entity =>
            {
                entity.ToTable("Podcast");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Title).IsRequired();
                entity.Property(p => p.Description).IsRequired(false);

                // Relation til Category
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Podcasts)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relation til Language
                entity.HasOne(p => p.Language)
                    .WithMany()
                    .HasForeignKey(p => p.LanguageId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Konfiguration af Season
            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("Season");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.ProductionYear).IsRequired();

                // Relation til Podcast
                entity.HasOne(s => s.Podcast)
                    .WithMany(p => p.Seasons)
                    .HasForeignKey(s => s.PodcastId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Konfiguration af Episode
            modelBuilder.Entity<Episode>(entity =>
            {
                entity.ToTable("Episode");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired(false);
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.AiringDate).IsRequired();

                // Relation til Season
                entity.HasOne(e => e.Season)
                    .WithMany(s => s.Episodes)
                    .HasForeignKey(e => e.SeasonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Konfiguration af Contribution
            modelBuilder.Entity<Contribution>(entity =>
            {
                entity.ToTable("Contribution");
                entity.HasKey(c => c.Id);

                // Relation til Contributor
                entity.HasOne(c => c.Contributor)
                    .WithMany()
                    .HasForeignKey(c => c.ContributorId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relation til Podcast (nullable)
                entity.HasOne(c => c.Podcast)
                    .WithMany(p => p.Hosts)
                    .HasForeignKey(c => c.PodcastId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);

                // Relation til Episode (nullable)
                entity.HasOne(c => c.Episode)
                    .WithMany(e => e.Contributions)
                    .HasForeignKey(c => c.EpisodeId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);

                // Relation til ContributorRole
                entity.HasOne(c => c.Role)
                    .WithMany()
                    .HasForeignKey(c => c.ContributorRoleId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Konfiguration af Contributor
            modelBuilder.Entity<Contributor>(entity =>
            {
                entity.ToTable("Contributor");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.Description).IsRequired(false);
            });

            // Konfiguration af ContributorRole
            modelBuilder.Entity<ContributorRole>(entity =>
            {
                entity.ToTable("ContributorRole");
                entity.HasKey(cr => cr.Id);
                entity.Property(cr => cr.Name).IsRequired();
            });

            // Konfiguration af Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.Description).IsRequired(false);
            });

            // Konfiguration af Language
            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Name).IsRequired();
                entity.Property(l => l.ISO).IsRequired();
            });

            // Konfiguration af Tag
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired();
            });

            // Konfiguration af mange-til-1 relation mellem Episode og Tag
            modelBuilder.Entity<Episode>()
                .HasOne(e => e.EpisodeTag)
                .WithMany(t => t.Episodes)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}