using MediatR;

namespace MusicCatalog.Features.ContributorRole.GetAll
{
    public class GetContributorRolesHandler : IRequestHandler<GetContributorRolesQuery, List<ContributorRoleDTO>>
    {
        private readonly IContributorRoleRepository _repository;

        public GetContributorRolesHandler(IContributorRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContributorRoleDTO>> Handle(GetContributorRolesQuery request, CancellationToken cancellationToken)
        {
            var contributorRoles = await _repository.GetAllAsync();

            return contributorRoles.Select(cr => new ContributorRoleDTO
                {
                    Id = cr.Id,
                    Name = cr.Name
                }).ToList();

        }
    }
}
