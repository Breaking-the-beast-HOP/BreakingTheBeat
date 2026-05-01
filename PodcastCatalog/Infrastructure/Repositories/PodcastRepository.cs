using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Infrastructure.Data;
using SharedKernel;

public class PodcastRepository : ICrudRepository<Podcast>
{
    private readonly PodcastCatalogDbContext _dbContext;

    public PodcastRepository(PodcastCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Podcast> AddAsync(Podcast entity)
    {
        await _dbContext.Podcasts.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Podcast?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Podcasts
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

    public async Task<bool> UpdateAsync(Podcast entity)
    {
        _dbContext.Podcasts.Update(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var podcast = await _dbContext.Podcasts.FindAsync(id);

        if (podcast == null)
            return false;

        _dbContext.Podcasts.Remove(podcast);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
