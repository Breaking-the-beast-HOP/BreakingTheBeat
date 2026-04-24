using SharedKernel.Interfaces.Base;

namespace MusicCatalog.Features.Genre.Interfaces
{
    public interface IGenreRepository : IReadRepository<Domain.Entities.Genre>
    {
    }
}
