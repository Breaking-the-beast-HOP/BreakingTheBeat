using MediatR;

namespace MusicCatalogContext.Features.Contributor.Get
{
    public record GetContributorsQuery : IRequest<List<ContributorDTO>>;
}
