using MediatR;

namespace MusicCatalogContext.Features.Countries.Get
{
    public record GetAllCountriesQuery : IRequest<List<CountryDTO>>;
}
