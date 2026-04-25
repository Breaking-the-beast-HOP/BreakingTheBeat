using MediatR;

namespace MusicCatalog.Features.Contributor.Get
{
    public record GetContributorsQuery : IRequest<List<ContributorDTO>>;
}
