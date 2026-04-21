using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Shared.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace PodcastCatalog.Shared.Infrastructure
{
    public class PodcastCatalogDbContext : DbContext
    {
        public PodcastCatalogDbContext(DbContextOptions<PodcastCatalogDbContext> options)
            : base(options)
        {
        }

        // Podcasts
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<PodcastContribution> PodcastContributions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Seasons
        public DbSet<Season> Seasons { get; set; }

        // Episodes
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<EpisodeContribution> EpisodeContributions { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // Contributors & Roles
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Podcast
            ConfigurePodcast(modelBuilder);

            // Languages (Many-to-Many)
            ConfigurePodcastLanguages(modelBuilder);

            // Categories (Many-to-Many)
            ConfigurePodcastCategories(modelBuilder);

            // Seasons
            ConfigureSeason(modelBuilder);

            // Episodes
            ConfigureEpisode(modelBuilder);

            // Contributions
            ConfigurePodcastContributions(modelBuilder);
            ConfigureEpisodeContributions(modelBuilder);

            // Tags (Many-to-Many)
            ConfigureEpisodeTags(modelBuilder);
        }


        // Podcast to Seasons: 1-to-many
        private void ConfigurePodcast(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Podcast>()
                .HasMany(p => p.Seasons)
                .WithOne(s => s.Podcast)
                .HasForeignKey(s => s.PodcastId);
        }

        // Season to Episodes: 1-to-many
        private void ConfigureEpisode(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Episode>()
                .HasOne(e => e.Season)
                .WithMany(s => s.Episodes)
                .HasForeignKey(e => e.SeasonId);
        }

        // Podcast to Languages: One-to-Many
        private void ConfigurePodcastLanguages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Podcast>()
                .HasOne(p => p.Language)
                .WithMany()
                .HasForeignKey("LanguageId");
        }

        // Podcast to Categories: Many-to-Many
        private void ConfigurePodcastCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Podcast>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Podcasts)
                .UsingEntity<PodcastCategories>(j => j.ToTable("PodcastCategories"));
        }

        // Season to Episodes: 1-to-many
        private void ConfigureSeason(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Season>()
                .HasMany(s => s.Episodes)
                .WithOne(e => e.Season)
                .HasForeignKey(e => e.SeasonId);
        }

        // Episode to Tags: Many-to-Many
        private void ConfigureEpisodeTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Episode>()
                .HasMany(e => e.Tags)
                .WithMany(t => t.Episodes)
                .UsingEntity(j => j.ToTable("EpisodeTags"));
        }

        // Episode to EpisodeContributions: 1-to-many
        private void ConfigureEpisodeContributions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Episode>()
                .HasMany(e => e.Contributions)
                .WithOne()
                .HasForeignKey(c => c.EpisodeId);
        }

        // Podcast to PodcastContributions: 1-to-many
        private void ConfigurePodcastContributions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Podcast>()
                .HasMany(p => p.Hosts)
                .WithOne(c => c.Podcast)
                .HasForeignKey(c => c.PodcastId);
        }
    }
}