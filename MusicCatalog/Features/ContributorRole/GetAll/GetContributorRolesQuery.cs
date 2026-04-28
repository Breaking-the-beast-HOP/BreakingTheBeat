using MediatR;

namespace MusicCatalog.Features.ContributorRole.GetAll
{
    public record GetContributorRolesQuery : IRequest<List<ContributorRoleDTO>>;

}