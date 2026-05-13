using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Infrastructure.Data;
using SharedKernel;

public class PodcastRepository : IPodcastRepository
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
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var podcast = await _dbContext.Podcasts.FindAsync(id);

        if (podcast == null)
            return false;

        _dbContext.Podcasts.Remove(podcast);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    // --- Seasons ---
    public async Task<Season> AddSeasonAsync(Guid podcastId, string? title, int seasonNumber)
    {
        var podcast = await _dbContext.Podcasts
            .Include(p => p.Seasons)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == podcastId);

        if (podcast is null)
            throw new KeyNotFoundException($"Podcast with ID {podcastId} not found.");

        var season = podcast.AddSeason(title, seasonNumber);

        _dbContext.Seasons.Add(season);
        await _dbContext.SaveChangesAsync();

        return season;
    }
    public async Task<bool> DeleteSeasonAsync(Guid seasonId)
    {
        var season = await _dbContext.Seasons
            .FirstOrDefaultAsync(s => s.Id == seasonId);

        if (season is null)
            return false;

        _dbContext.Seasons.Remove(season);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    // --- Episodes ---

    public async Task<Episode> AddEpisodeAsync(
      Guid seasonId,
      string? title,
      string? description,
      TimeSpan? duration,
      DateTime? airingDate,
      Guid? tagId)
    {
        var season = await _dbContext.Seasons
            .Include(s => s.Episodes)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == seasonId);

        if (season is null)
            throw new KeyNotFoundException($"Season with ID {seasonId} not found.");

        var episode = season.AddEpisode(title, description, duration, airingDate, tagId);

        _dbContext.Episodes.Add(episode);
        await _dbContext.SaveChangesAsync();

        return episode;
    }

    public async Task<bool> DeleteEpisodeAsync(Guid episodeId)
    {
        var episode = await _dbContext.Episodes
            .FirstOrDefaultAsync(e => e.Id == episodeId);

        if (episode is null)
            return false;

        _dbContext.Episodes.Remove(episode);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
