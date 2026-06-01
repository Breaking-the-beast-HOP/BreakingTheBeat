using MediatR;

namespace MusicQualityControl.Features.DeleteReleaseFromQc;

public record DeleteReleaseFromQcCommand(Guid ReleaseId) : IRequest<bool>;