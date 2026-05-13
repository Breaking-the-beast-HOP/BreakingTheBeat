using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Features.Podcasts.Interfaces
{
    public interface IPodcastRepository: ICrudRepository<Domain.Entities.Podcast>
    {
        Task<Season> AddSeasonAsync(Guid podcastId, string? title, int seasonNumber);
        Task<bool> DeleteSeasonAsync(Guid seasonId);
        Task<Episode> AddEpisodeAsync(
            Guid seasonId,
            string? title,
            string? description,
            TimeSpan? duration,
            DateTime? airingDate,
            Guid? tagId);
        Task<bool> DeleteEpisodeAsync(Guid episodeId);
        
    }
}
