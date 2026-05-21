using MediatR;

namespace MusicCatalogContext.Features.Track.GetById
{
    public record GetTrackByIdQuery(Guid Id) : IRequest<TrackDTO>;
}
