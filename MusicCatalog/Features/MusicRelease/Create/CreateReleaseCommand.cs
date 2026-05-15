using MediatR;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Domain.Enums;

namespace MusicCatalog.Features.Release.Create;

public record CreateReleaseCommand(
    string Title,
    string? Version,
    string? EAN,
    ReleaseStatus Status,
    DateTime? ReleaseDate,
    Guid GenreId,
    Guid LanguageId,
    List<CreateReleaseVolumeInput> Volumes,
    List<CreateReleaseContributionInput> Contributions
) : IRequest<Guid>;

public record CreateReleaseContributionInput(Guid ContributorId, List<Guid> RoleIds);

public record CreateReleaseVolumeInput(int VolumeNumber, List<CreateReleaseTrackInput> Tracks);

public record CreateReleaseTrackInput(int TrackNumber, Guid TrackId);
