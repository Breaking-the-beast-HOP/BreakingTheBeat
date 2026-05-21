using MediatR;

namespace MusicQualityControl.Features.SubmitReleaseToQc
{
    public record SubmitReleaseToQcCommand(Guid ReleaseId, string Title, string? Version, string? EAN) : IRequest;
}