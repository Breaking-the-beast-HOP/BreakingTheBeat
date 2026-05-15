using Frontend.Components.Pages.MusicCatalog.SharedModels;
using static Frontend.Components.Pages.MusicCatalog.MusicReleases.Create.CreateRelease;

namespace Frontend.Components.Pages.MusicCatalog.MusicReleases.Models;

public class ReleaseModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? EAN { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public ReleaseStatusDto Status { get; set; } = ReleaseStatusDto.Draft;

    public Guid? GenreId { get; set; }
    public Guid? LanguageId { get; set; }

    public List<ReleaseVolumeModel> Volumes { get; set; } = new();
    public List<ContributionRequest> Contributions { get; set; } = new();
    
}

public class ReleaseVolumeModel
{
    public Guid Id { get; set; }
    public int VolumeNumber { get; set; }
    public List<ReleaseTrackModel> Tracks { get; set; } = new();
}

public class ReleaseTrackModel
{
    public int TrackNumber { get; set; }
    public Guid? TrackId { get; set; }
    public TrackDto? SelectedTrack { get; set; }
}

public class ReleaseListDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? EAN { get; set; }
    public DateTime? ReleaseDate { get; set; }

    public string? GenreName { get; set; }
    public string? LanguageName { get; set; }
}

public class ReleaseDetailsDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? EAN { get; set; }
    public DateTime? ReleaseDate { get; set; }

    public Guid? GenreId { get; set; }
    public string? GenreName { get; set; }

    public Guid? LanguageId { get; set; }
    public string? LanguageName { get; set; }

    public bool HasCover { get; set; }
    public bool HasBooklet { get; set; }

    public List<ReleaseVolumeDto> Volumes { get; set; } = new();
    public List<ContributionDto> Contributions { get; set; } = new();
}

public class ReleaseVolumeDto
{
    public Guid Id { get; set; }
    public int VolumeNumber { get; set; }
    public List<ReleaseTrackDto> Tracks { get; set; } = new();
}

public class ReleaseTrackDto
{
    public Guid TrackId { get; set; }
    public int TrackNumber { get; set; }

    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? ISRC { get; set; }
    public int DurationSeconds { get; set; }
}

public enum ReleaseStatusDto
{
    Draft,
    ReadyForQc,
    InQc,
    Approved,
    Rejected,
    Distributed
}