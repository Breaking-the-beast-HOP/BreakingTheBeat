using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Features.Contributors.GetAll
{
    public class GetAllContributorsHandler : IRequestHandler<GetAllContributorsQuery, List<ContributorDTO>>
    {
        private readonly ICrudRepository<Contributor> _contributorRepository;

        public GetAllContributorsHandler(ICrudRepository<Contributor> contributorRepository)
        {
            _contributorRepository = contributorRepository;
        }

        public async Task<List<ContributorDTO>> Handle(GetAllContributorsQuery request, CancellationToken cancellationToken)
        {
            var contributors = await _contributorRepository.GetAllAsync();
            if (contributors == null || !contributors.Any())
            {
                return new List<ContributorDTO>
                {
                    new ContributorDTO
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Placeholder",
                        Description = "Placeholder"
                    }
                };
            }
            var contributorDTOs = contributors.Select(c => new ContributorDTO
            {
                Id = c.Id,
                Name = c.DisplayName,
                Description = c.Biography
            }).ToList();
            return contributorDTOs;
        }
    }
}
