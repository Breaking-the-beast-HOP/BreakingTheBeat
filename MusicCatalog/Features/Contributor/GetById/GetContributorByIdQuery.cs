using MediatR;

namespace MusicCatalogContext.Features.Contributor.GetById
{
    public record GetContributorByIdQuery(Guid Id) : IRequest<ContributorDetailsDTO>;
}