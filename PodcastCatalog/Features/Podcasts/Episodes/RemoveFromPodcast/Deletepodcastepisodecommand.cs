using MediatR;

namespace PodcastCatalog.Features.Podcasts.Episodes.RemoveFromPodcast
{

    public record DeletePodcastEpisodeCommand(Guid EpisodeId) : IRequest<bool>;
}
