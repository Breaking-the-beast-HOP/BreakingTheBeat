using MediatR;

namespace MusicCatalogContext.Features.Countries.Get
{
    
    public class GetAllCountriesHandler : IRequestHandler<GetAllCountriesQuery, List<CountryDTO>>
    {
        private readonly ICountryRepository _repository;

        public GetAllCountriesHandler(ICountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CountryDTO>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _repository.GetAllAsync();

            return countries.Select(l => new CountryDTO
            {
                Id = l.Id,
                Name = l.Name,
                ISO = l.ISO
            }).ToList();
        }
    }
}
