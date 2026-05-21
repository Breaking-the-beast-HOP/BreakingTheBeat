using MediatR;

namespace MusicCatalogContext.Features.Release.GetAll
{
    public class GetAllReleasesHandler : IRequestHandler<GetAllReleasesQuery, List<ReleaseDto>>
    {
        private readonly IReleaseRepository _repository;

        public GetAllReleasesHandler(IReleaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReleaseDto>> Handle(
            GetAllReleasesQuery request,
            CancellationToken cancellationToken)
        {
            var releases = await _repository.GetAllAsync();

            return releases.Select(r => new ReleaseDto
            {
                Id = r.Id,
                Title = r.Title,
                Version = r.Version,
                EAN = r.EAN,
                ReleaseDate = r.ReleaseDate,
                GenreName = r.Genre.Name,
                LanguageName = r.Language.Name
            }).ToList();
        }
    }
}