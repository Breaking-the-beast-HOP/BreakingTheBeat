
using MediatR;

namespace MusicCatalog.Features.Release.Create
{
    public record CreateReleaseCommand(
        string Title,
        string? Version,
        string? EAN,
        DateTime? ReleaseDate,
        Guid GenreId,
        Guid LanguageId,
        List<CreateReleaseVolumeCommand> Volumes,
        List<CreateReleaseContributionCommand> Contributions
    ) : IRequest<Guid>;


}
