using MediatR;

namespace MusicCatalog.Features.Track.Delete
{
    public record DeleteTrackCommand(Guid Id) : IRequest<bool>;
}
