using MediatR;
using MusicCatalog.Features.Language.Interfaces;

namespace MusicCatalog.Features.Contributor.Get
{
    public class GetContributorsHandler: IRequestHandler<GetContributorsQuery, List<ContributorDTO>>
    {
        private readonly IContributorRepository _repository;
        public GetContributorsHandler(IContributorRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<ContributorDTO>> Handle(GetContributorsQuery request, CancellationToken cancellationToken)
        {
            var contributors = await _repository.GetAllAsync();
            return contributors.Select(c => new ContributorDTO
            {
                Id = c.Id,
                DisplayName = c.DisplayName,
                LegalName = c.LegalName,
                IPI = c.IPI,
                ISNI = c.ISNI
            }).ToList();
        }
    }
}
