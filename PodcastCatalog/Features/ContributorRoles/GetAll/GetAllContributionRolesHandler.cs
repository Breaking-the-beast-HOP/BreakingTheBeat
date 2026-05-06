using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.ContributorRoles.GetAll
{
    public class GetAllContributionRolesHandler : IRequestHandler<GetAllContributionRolesQuery, List<ContributorRoleDTO>>
    {
        private readonly IReadRepository<ContributorRole> _contributorRoleRepository;
        public GetAllContributionRolesHandler(IReadRepository<ContributorRole> contributorRoleRepository)
        {
            _contributorRoleRepository = contributorRoleRepository;
        }
        public async Task<List<ContributorRoleDTO>> Handle(GetAllContributionRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _contributorRoleRepository.GetAllAsync();
            if (roles == null || !roles.Any())
            {
                return new List<ContributorRoleDTO>
                {
                    new ContributorRoleDTO
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Placeholder",
                        Description = "Placeholder"
                    }
                };
            }

            var ContributorRoleDTOs = roles.Select(r => new ContributorRoleDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();
            return ContributorRoleDTOs;
        }
    }
}
