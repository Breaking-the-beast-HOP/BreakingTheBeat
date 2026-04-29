using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Infrastructure.Data;
using SharedKernel;

namespace PodcastCatalog.Shared.Infrastructure.Repositories
{
    public class PodcastRepository : ICrudRepository<Podcast>
    {
        private readonly PodcastCatalogDbContext _dbContext;

        public PodcastRepository(PodcastCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ICreateRepository<Podcast>
        public async Task<Podcast> AddAsync(Podcast entity)
        {
            await _dbContext.Podcasts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        // IReadRepository<Podcast>
        public async Task<Podcast?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Podcasts
                .AsNoTracking()
                .Include(p => p.Seasons)
                .Include(p => p.Category)
                .Include(p => p.Hosts)
                .Include(p => p.Language)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Podcast>> GetAllAsync()
        {
            return await _dbContext.Podcasts
                .AsNoTracking()
                .Include(p => p.Seasons)
                .Include(p => p.Category)
                .Include(p => p.Hosts)
                .Include(p => p.Language)
                .ToListAsync();
        }

        // IUpdateRepository<Podcast>
        public async Task UpdateAsync(Podcast entity)
        {
            _dbContext.Podcasts.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        // IDeleteRepository<Podcast>
        public async Task DeleteAsync(Podcast entity)
        {
            _dbContext.Podcasts.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}