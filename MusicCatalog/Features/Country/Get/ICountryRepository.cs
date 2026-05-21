using MusicCatalogContext.Domain.Entities;
using SharedKernel.Interfaces.Base;

namespace MusicCatalogContext.Features.Countries.Get
{
    public interface ICountryRepository : IReadRepository<Country>
    {
    }
}