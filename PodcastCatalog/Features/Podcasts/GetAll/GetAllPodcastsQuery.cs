using MediatR;

namespace PodcastCatalog.Features.Podcasts.GetAll
{
    public record GetAllPodcastsQuery : IRequest<List<PodcastDTO>>;
}
