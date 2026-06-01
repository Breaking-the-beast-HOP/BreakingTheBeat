using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Infrastructure
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
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(PodcastCatalogDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}