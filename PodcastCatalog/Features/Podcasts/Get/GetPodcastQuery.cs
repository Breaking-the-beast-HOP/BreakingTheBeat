using MediatR;

namespace PodcastCatalog.Features.Podcasts.Get
{
    public record GetPodcastQuery(Guid Id) : IRequest<PodcastDTO>;
}
