using MediatR;
using MusicQualityControl.Domain.ReadModels;

namespace MusicQualityControl.Features.DeleteReleaseFromQc;

public class DeleteReleaseFromQcHandler : IRequestHandler<DeleteReleaseFromQcCommand, bool>
{
    private readonly IReleaseQcQueueItemRepository _repository;

    public DeleteReleaseFromQcHandler(IReleaseQcQueueItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteReleaseFromQcCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.ReleaseId);
    }
}