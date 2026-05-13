using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.GetAll
{
    public record GetAllPodcastsQuery : IRequest<List<PodcastDTO>>;
}
