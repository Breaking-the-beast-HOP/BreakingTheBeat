using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;

namespace PodcastCatalog.Infrastructure.Repositories;

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
            .Include(p => p.Seasons)
                .ThenInclude(s => s.Episodes)
                    .ThenInclude(e => e.Contributions)
                        .ThenInclude(c => c.Contributor)
            .Include(p => p.Seasons)
                .ThenInclude(s => s.Episodes)
                    .ThenInclude(e => e.Contributions)
                        .ThenInclude(c => c.Role)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Podcast>> GetAllAsync()
    {
        return await _dbContext.Podcasts
            .AsNoTracking()
            .Include(p => p.Seasons)
            .Include(p => p.Category)
            .Include(p => p.Language)
            .Include(p => p.Contributions)
                .ThenInclude(c => c.Contributor)
            .Include(p => p.Contributions)
                .ThenInclude(c => c.Role)
            .ToListAsync();
    }

    public async Task<Podcast?> GetByIdForUpdateAsync(Guid id)
    {
        return await _dbContext.Podcasts
            .Include(p => p.Contributions)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> UpdateAsync(Podcast entity)
    {
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var podcast = await _dbContext.Podcasts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (podcast is null)
            return false;

        _dbContext.Podcasts.Remove(podcast);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Season> AddSeasonAsync(Guid podcastId, string? title, int seasonNumber)
    {
        var podcast = await _dbContext.Podcasts
            .Include(p => p.Seasons)
            .FirstOrDefaultAsync(p => p.Id == podcastId);

         if (podcast is null)
            throw new KeyNotFoundException($"Podcast with ID {podcastId} not found.");

        var season = podcast.AddSeason(title, seasonNumber);

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
            .FirstOrDefaultAsync(s => s.Id == seasonId);

        if (season is null)
            throw new KeyNotFoundException($"Season with ID {seasonId} not found.");

        var episode = season.AddEpisode(
            title,
            description,
            duration,
            airingDate,
            tagId);

        _dbContext.Entry(episode).State = EntityState.Added;

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