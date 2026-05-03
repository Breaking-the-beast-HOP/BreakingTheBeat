using MediatR;

namespace MusicCatalog.Features.Contributor.GetById
{
    public record GetContributorByIdQuery(Guid Id) : IRequest<ContributorDetailsDTO>;
}