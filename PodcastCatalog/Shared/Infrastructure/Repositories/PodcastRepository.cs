using PodcastCatalog.Shared.Models;

namespace PodcastCatalog.Shared.Infrastructure.Repositories
{
    public class PodcastRepository : IPodcastRepository
    {
        private readonly PodcastCatalogDbContext _dbContext;

        public PodcastRepository(PodcastCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Podcast podcast, CancellationToken ct)
        {
            await _dbContext.Podcasts.AddAsync(podcast, ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        public Task DeleteAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Podcast?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        //public async Task<Podcast?> GetByIdAsync(Guid id, CancellationToken ct)
        //{
        //    return await _dbContext.Podcasts
        //        .Include(p => p.Seasons)
        //        .Include(p => p.Languages)
        //        .Include(p => p.Categories)
        //        .Include(p => p.Hosts)
        //        .FirstOrDefaultAsync(p => p.Id == id, ct);
        //}

        public Task UpdateAsync(Podcast podcast, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
