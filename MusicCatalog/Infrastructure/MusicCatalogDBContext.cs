using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Domain;
using MusicCatalogContext.Domain.Entities;

namespace MusicCatalogContext.Infrastructure
{
    public class MusicCatalogDbContext : DbContext
    {
        public MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options) : base(options)
        {
        }
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Track> Tracks => Set<Track>();
        public DbSet<Contributor> Contributors => Set<Contributor>();
        public DbSet<ContributorRole> ContributorRoles => Set<ContributorRole>();
        public DbSet<Contribution> Contributions => Set<Contribution>();
        public DbSet<Release> Releases => Set<Release>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        public DbSet<Label> Labels => Set<Label>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicCatalogDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}