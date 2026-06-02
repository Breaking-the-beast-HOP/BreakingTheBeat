using MediatR;
using MusicQualityControl.Domain.ReadModels;

namespace MusicQualityControl.Features.DeleteReleaseFromQc;

public class WithdrawReleaseFromQcHandler : IRequestHandler<WithdrawReleaseFromQcCommand, bool>
{
    private readonly IReleaseQcQueueItemRepository _repository;

    public WithdrawReleaseFromQcHandler(IReleaseQcQueueItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(WithdrawReleaseFromQcCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.ReleaseId);
    }
}