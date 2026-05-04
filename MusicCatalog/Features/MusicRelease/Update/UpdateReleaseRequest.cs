using MediatR;

namespace MusicCatalog.Features.Release.Update;

public record UpdateReleaseCommand(
    Guid Id,
    string? Title,
    string? Version,
    string? EAN,
    DateTime? ReleaseDate,
    Guid? GenreId,
    Guid? MetadataLanguageId,
    List<UpdateReleaseVolumeInput> Volumes,
    List<UpdateReleaseContributionInput> Contributions
) : IRequest<bool>;



public record UpdateReleaseVolumeInput(int VolumeNumber, List<UpdateReleaseTrackInput> Tracks);

public record UpdateReleaseTrackInput(Guid TrackId, int TrackNumber);

public record UpdateReleaseContributionInput(Guid ContributorId, Guid ContributorRoleId);