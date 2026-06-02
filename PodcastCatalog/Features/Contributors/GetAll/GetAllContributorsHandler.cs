using MediatR;

namespace PodcastCatalog.Features.Contributors.GetAll
{
    public class GetAllContributorsHandler
        : IRequestHandler<GetAllContributorsQuery, List<ContributorDTO>>
    {
        private readonly IContributorRepository _repository;

        public GetAllContributorsHandler(IContributorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContributorDTO>> Handle(GetAllContributorsQuery request, CancellationToken cancellationToken)
        {
            var contributors = await _repository.GetAllAsync();

            return contributors
            .Select(c => new ContributorDTO(
                c.Id,
                c.DisplayName,
                c.FullName,
                c.Title,
                c.Biography,
                c.Expertise
            ))
            .ToList();
        }
    }
}