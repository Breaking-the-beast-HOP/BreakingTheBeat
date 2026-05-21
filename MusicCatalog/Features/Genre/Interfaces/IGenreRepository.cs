using SharedKernel.Interfaces.Base;

namespace MusicCatalogContext.Features.Genre.Interfaces
{
    public interface IGenreRepository : IReadRepository<Domain.Entities.Genre>
    {
    }
}
