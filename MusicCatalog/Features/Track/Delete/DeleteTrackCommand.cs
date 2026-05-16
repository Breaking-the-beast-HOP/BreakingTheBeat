using MediatR;

namespace MusicCatalogContext.Features.Track.Delete
{
    public record DeleteTrackCommand(Guid Id) : IRequest<bool>;
}
