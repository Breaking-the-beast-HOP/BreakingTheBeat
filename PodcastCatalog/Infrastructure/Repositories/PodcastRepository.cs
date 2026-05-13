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
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Language)
                .Include(p => p.Contributions)
                    .ThenInclude(c => c.Contributor)
                .Include(p => p.Contributions)
                    .ThenInclude(c => c.Role)
                .Include(p => p.Seasons)
                    .ThenInclude(s => s.Episodes)
                        .ThenInclude(e => e.EpisodeTag)
                .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Podcast>> GetAllAsync()
    {
        return await _dbContext.Podcasts
            .AsNoTracking()
            .Include(p => p.Seasons)
            .Include(p => p.Category)
            .Include(p => p.Contributions)
            .Include(p => p.Language)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Podcast entity)
    {
        // This is an anti-pattern, so contributions will be updated separately.
        // This must be fixed in the future, but for now, we will just add new contributions and remove missing ones.
        var existingContributions = await _dbContext.Contributions
            .Where(c => c.PodcastId == entity.Id)
            .ToListAsync();
        foreach (var contribution in entity.Contributions)
        {
            if (!existingContributions.Any(c => c.Id == contribution.Id))
            {
                _dbContext.Contributions.Add(contribution);
            }
        }
        foreach (var existingContribution in existingContributions)
        {
            if (!entity.Contributions.Any(c => c.Id == existingContribution.Id))
            {
                _dbContext.Contributions.Remove(existingContribution);
            }
        }
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
