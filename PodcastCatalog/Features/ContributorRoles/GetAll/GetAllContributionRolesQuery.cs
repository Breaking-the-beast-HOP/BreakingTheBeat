using MediatR;

namespace PodcastCatalog.Features.ContributorRoles.GetAll
{
    public record GetAllContributionRolesQuery : IRequest<List<ContributorRoleDTO>>
    {
    }
}
