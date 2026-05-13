using MediatR;

namespace PodcastCatalog.Features.Contributors.GetAll
{
    public record GetAllContributorsQuery : IRequest<List<ContributorDTO>>
    {
    }
}
