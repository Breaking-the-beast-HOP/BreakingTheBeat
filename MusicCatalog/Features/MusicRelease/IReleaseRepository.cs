using SharedKernel;

namespace MusicCatalogContext.Features.MusicRelease
{
    public interface IReleaseRepository : ICrudRepository<MusicCatalogContext.Domain.Entities.Release>
    {
    }
}