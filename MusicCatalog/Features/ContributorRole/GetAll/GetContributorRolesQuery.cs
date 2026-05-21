using MediatR;

namespace MusicCatalogContext.Features.ContributorRole.GetAll
{
    public record GetContributorRolesQuery : IRequest<List<ContributorRoleDTO>>;

}