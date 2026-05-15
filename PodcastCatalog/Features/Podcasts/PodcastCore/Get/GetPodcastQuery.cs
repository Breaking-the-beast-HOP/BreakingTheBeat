using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.Get
{
    public record GetPodcastQuery(Guid Id) : IRequest<PodcastDTO>;
}
