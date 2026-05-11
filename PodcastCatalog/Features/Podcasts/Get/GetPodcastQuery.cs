using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Get
{
    public record GetPodcastQuery(Guid Id) : IRequest<PodcastDTO>;
}
