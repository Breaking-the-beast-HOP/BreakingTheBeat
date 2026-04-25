using MediatR;
using MusicCatalog.Features.ContributorRole;

namespace MusicCatalog.Features.ContributorRole.GetAll
{
    public record GetContributorRolesQuery : IRequest<List<ContributorRoleDTO>>;
    
}