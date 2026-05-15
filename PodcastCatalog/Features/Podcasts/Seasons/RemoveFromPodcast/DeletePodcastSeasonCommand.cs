using MediatR;

namespace PodcastCatalog.Features.Podcasts.Seasons.RemoveFromPodcast
{
    public record DeletePodcastSeasonCommand(Guid SeasonId) : IRequest<bool>;
}
