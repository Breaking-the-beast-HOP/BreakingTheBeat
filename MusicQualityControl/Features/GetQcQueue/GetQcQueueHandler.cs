using MediatR;
using MusicQualityControl.Domain.ReadModels;
using MusicQualityControl.Features;

namespace MusicQualityControl.Features.GetQcQueue;

public class GetQcQueueHandler : IRequestHandler<GetQcQueueQuery, List<ReleaseQcQueueItem>>
{
    private readonly IReleaseQcQueueItemRepository _repository;

    public GetQcQueueHandler(IReleaseQcQueueItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ReleaseQcQueueItem>> Handle(GetQcQueueQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();

    }
}