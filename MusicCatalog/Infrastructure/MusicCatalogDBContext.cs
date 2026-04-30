using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure
{
    public class MusicCatalogDbContext : DbContext
    {
        public MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options) : base(options)
        {
        }
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Track> Tracks => Set<Track>();
        public DbSet<Contributor> Contributors => Set<Contributor>();
        public DbSet<ContributorRole> ContributorRoles => Set<ContributorRole>();
        public DbSet<Contribution> Contributions => Set<Contribution>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicCatalogDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}