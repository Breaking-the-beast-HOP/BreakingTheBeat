using MediatR;

namespace MusicCatalogContext.Features.Contributor.GetById
{
    public class GetContributorByIdHandler
        : IRequestHandler<GetContributorByIdQuery, ContributorDetailsDTO>
    {
        private readonly IContributorRepository _repository;

        public GetContributorByIdHandler(IContributorRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContributorDetailsDTO> Handle(GetContributorByIdQuery request, CancellationToken cancellationToken)
        {
            var contributor = await _repository.GetByIdAsync(request.Id);

            if (contributor is null)
                throw new Exception("Contributor not found.");

            var contributions = await _repository.GetContributionsByContributorIdAsync(request.Id);

            return new ContributorDetailsDTO
            {
                Id = contributor.Id,
                DisplayName = contributor.DisplayName ?? "",
                LegalName = contributor.LegalName,
                IPI = contributor.IPI,
                ISNI = contributor.ISNI,

                TrackContributions = contributions
                    .Where(c => c.TrackId is not null)
                    .Select(c => new ContributorTrackContributionDTO
                    {
                        TrackId = c.TrackId!.Value,
                        TrackTitle = c.Track?.Title ?? "",
                        ISRC = c.Track?.ISRC,
                        RoleName = c.Role.Name
                    })
                    .ToList(),

                ReleaseContributions = contributions
                    .Where(c => c.ReleaseId is not null)
                    .Select(c => new ContributorReleaseContributionDTO
                    {
                        ReleaseId = c.ReleaseId!.Value,
                        ReleaseTitle = c.Release?.Title ?? "",
                        EAN = c.Release?.EAN,
                        RoleName = c.Role.Name
                    })
                    .ToList()
            };
        }
    }
}