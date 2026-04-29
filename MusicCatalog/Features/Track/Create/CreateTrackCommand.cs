using MediatR;

namespace MusicCatalog.Features.Track.Create
{
    public record CreateTrackCommand(string Title, string Version, string ISRC, Guid GenreId, Guid AudioLanguageId, Guid MetadataLanguageId, List<CreateContributionCommand> Contributions) : IRequest<Guid>;
}
