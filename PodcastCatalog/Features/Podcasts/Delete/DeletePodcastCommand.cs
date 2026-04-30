using MediatR;

namespace PodcastCatalog.Features.Podcasts.Delete
{
    public record DeletePodcastCommand(Guid Id) : IRequest<Unit>;
}
