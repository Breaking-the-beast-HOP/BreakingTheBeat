using MediatR;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.Delete
{
    public record DeletePodcastCommand(Guid Id) : IRequest<Unit>;
}
