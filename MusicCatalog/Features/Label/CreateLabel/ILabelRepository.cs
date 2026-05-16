using SharedKernel;

namespace MusicCatalog.Features.Label.Create
{
    public interface ILabelRepository: ICrudRepository<MusicCatalogContext.Domain.Entities.Label>
    {
    }
}