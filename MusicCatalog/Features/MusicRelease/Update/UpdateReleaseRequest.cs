using MediatR;
using MusicCatalogContext.Domain.Enums;

namespace MusicCatalogContext.Features.MusicRelease.Update;

public record UpdateReleaseCommand(
    Guid Id,
    string? Title,
    string? Version,
    string? EAN,
    DateTime? ReleaseDate,
    Guid? GenreId,
    Guid? MetadataLanguageId,
    ReleaseStatus Status,
    List<UpdateReleaseVolumeInput> Volumes,
    List<UpdateReleaseContributionInput> Contributions
) : IRequest<bool>;



public record UpdateReleaseVolumeInput(int VolumeNumber, List<UpdateReleaseTrackInput> Tracks);

public record UpdateReleaseTrackInput(Guid TrackId, int TrackNumber);

public record UpdateReleaseContributionInput(Guid ContributorId, Guid ContributorRoleId);