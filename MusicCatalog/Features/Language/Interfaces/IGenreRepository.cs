using SharedKernel.Interfaces.Base;

namespace MusicCatalog.Features.Language.Interfaces
{
    public interface IGenreRepository: IReadRepository<Domain.Entities.Genre>
    {
    }
}
