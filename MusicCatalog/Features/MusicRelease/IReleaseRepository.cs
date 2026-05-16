using SharedKernel;

namespace MusicCatalogContext.Features.Release
{
    public interface IReleaseRepository : ICrudRepository<MusicCatalogContext.Domain.Entities.Release>
    {
    }
}