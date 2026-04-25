using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Features.ContributorRole.GetAll
{
    public class GetContributorRoleHandler : IRequestHandler<GetContributorRolesQuery, List<ContributorRoleDTO>>
    {
        private readonly IContributorRoleRepository _repository;

        public GetContributorRoleHandler(IContributorRoleRepository repository)
        {
            _repository = repository;
        }

        public Task<List<ContributorRoleDTO>> Handle(GetContributorRolesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
