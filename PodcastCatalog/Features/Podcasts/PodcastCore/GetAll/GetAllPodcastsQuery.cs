using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.GetAll
{
    public record GetAllPodcastsQuery : IRequest<List<PodcastDTO>>;
}
