using MediatR;
using MusicQualityControl.Domain.ReadModels;

namespace MusicQualityControl.Features.SubmitReleaseToQc;

public class SubmitReleaseToQcHandler : IRequestHandler<SubmitReleaseToQcCommand>
{
    private readonly IReleaseQcQueueItemRepository _repository;

    public SubmitReleaseToQcHandler(IReleaseQcQueueItemRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(SubmitReleaseToQcCommand request, CancellationToken cancellationToken)
    {
        var item = ReleaseQcQueueItem.Create(
            request.ReleaseId,
            request.Title,
            request.Version,
            request.EAN
        );

        await _repository.AddAsync(item);
    }
}