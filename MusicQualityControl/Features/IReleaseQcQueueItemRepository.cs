using MusicQualityControl.Domain.ReadModels;
using SharedKernel.Interfaces.Base;

namespace MusicQualityControl.Features
{
    public interface IReleaseQcQueueItemRepository : IReadRepository<ReleaseQcQueueItem>, ICreateRepository<ReleaseQcQueueItem>, IDeleteRepository<ReleaseQcQueueItem>
    {
    }
}
