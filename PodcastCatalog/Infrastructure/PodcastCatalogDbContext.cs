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

            ConfigurePodcast(modelBuilder);
            ConfigureSeason(modelBuilder);
            ConfigureEpisode(modelBuilder);
            ConfigureContribution(modelBuilder);
            ConfigureContributor(modelBuilder);
            ConfigureContributorRole(modelBuilder);
            ConfigureCategory(modelBuilder);
            ConfigureLanguage(modelBuilder);
            ConfigureTag(modelBuilder);
        }

        private static void ConfigurePodcast(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Podcast>(entity =>
            {
                entity.ToTable("Podcast");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(p => p.Description)
                    .IsRequired(false);

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Podcasts)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.Language)
                    .WithMany()
                    .HasForeignKey(p => p.LanguageId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Metadata
                    .FindNavigation(nameof(Podcast.Seasons))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.Metadata
                    .FindNavigation(nameof(Podcast.Contributions))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
            });
        }

        private static void ConfigureSeason(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("Season");

                entity.HasKey(s => s.Id);

                entity.Property(s => s.Title)
                    .IsRequired(false)
                    .HasColumnType("text");

                entity.Property(s => s.SeasonNumber)
                    .IsRequired();

                entity.Property(s => s.ReleaseDate)
                    .IsRequired(false)
                    .HasColumnType("date");

                entity.HasIndex(s => new
                {
                    s.PodcastId,
                    s.SeasonNumber
                }).IsUnique();

                entity.HasOne(s => s.Podcast)
                    .WithMany(p => p.Seasons)
                    .HasForeignKey(s => s.PodcastId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Metadata
                    .FindNavigation(nameof(Season.Episodes))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
            });
        }

        private static void ConfigureEpisode(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Episode>(entity =>
            {
                entity.ToTable("Episode");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired(false)
                    .HasMaxLength(250);

                entity.Property(e => e.Description)
                    .IsRequired(false);

                entity.Property(e => e.Duration)
                    .IsRequired(false);

                entity.Property(e => e.AiringDate)
                    .IsRequired(false);

                entity.Property(e => e.EpisodeNumber)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => new
                {
                    e.SeasonId,
                    e.EpisodeNumber
                }).IsUnique();

                entity.HasOne(e => e.Season)
                    .WithMany(s => s.Episodes)
                    .HasForeignKey(e => e.SeasonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.EpisodeTag)
                    .WithMany(t => t.Episodes)
                    .HasForeignKey(e => e.TagId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);

                entity.Metadata
                    .FindNavigation(nameof(Episode.Contributions))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
            });
        }

        private static void ConfigureContribution(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contribution>(entity =>
            {
                entity.ToTable("Contribution");

                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.Contributor)
                    .WithMany()
                    .HasForeignKey(c => c.ContributorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Podcast)
                    .WithMany(p => p.Contributions)
                    .HasForeignKey(c => c.PodcastId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                entity.HasOne(c => c.Episode)
                    .WithMany(e => e.Contributions)
                    .HasForeignKey(c => c.EpisodeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                entity.HasOne(c => c.Role)
                    .WithMany()
                    .HasForeignKey(c => c.ContributorRoleId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Property(c => c.HasRights)
                    .IsRequired();
            });
        }

        private static void ConfigureContributor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contributor>(entity =>
            {
                entity.ToTable("Contributor");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(c => c.Description)
                    .IsRequired(false);
            });
        }

        private static void ConfigureContributorRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContributorRole>(entity =>
            {
                entity.ToTable("ContributorRole");

                entity.HasKey(cr => cr.Id);

                entity.Property(cr => cr.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(cr => cr.Description)
                    .IsRequired(false);
            });
        }

        private static void ConfigureCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(c => c.Description)
                    .IsRequired(false);
            });
        }

        private static void ConfigureLanguage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");

                entity.HasKey(l => l.Id);

                entity.Property(l => l.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(l => l.ISO)
                    .IsRequired()
                    .HasMaxLength(10);
            });
        }

        private static void ConfigureTag(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });
        }
    }
}