using MediatR;

namespace MusicCatalog.Features.Track.GetById
{
    public record GetTrackByIdQuery(Guid Id) : IRequest<TrackDTO>;
}
