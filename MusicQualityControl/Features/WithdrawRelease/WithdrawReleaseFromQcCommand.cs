using MediatR;

namespace MusicQualityControl.Features.DeleteReleaseFromQc;

public record WithdrawReleaseFromQcCommand(Guid ReleaseId) : IRequest<bool>;