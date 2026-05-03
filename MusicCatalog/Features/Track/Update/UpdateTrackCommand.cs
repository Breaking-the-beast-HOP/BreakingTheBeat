using MediatR;

namespace MusicCatalog.Features.Track.Update
{
    public record UpdateTrackCommand( Guid Id,string? Title,string? Version,string? ISRC,Guid? GenreId,Guid? MetadataLanguageId,Guid? AudioLanguageId, List<UpdateContributionCommand> Contributions) : IRequest<bool>;
}
