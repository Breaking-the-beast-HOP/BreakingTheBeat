using FrontendThrowAway.Components.Pages.MusicCatalog.Tracks.Models;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.SharedModels;

public class GenreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
}

public class LanguageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string ISO { get; set; } = "";
}

public class ContributionDto
{
    public Guid ContributorId { get; set; }
    public string ContributorName { get; set; } = "";
    public List<ContributionRoleDto> Roles { get; set; } = new();
}

public class ContributionRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
}

public class ContributionRequest
{
    public Guid ContributorId { get; set; }
    public List<Guid> RoleIds { get; set; } = new();
}

public class TrackDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? ISRC { get; set; }
    public int DurationSeconds { get; set; }

    public string? GenreName { get; set; }
    public string? AudioLanguageName { get; set; }
}

public class TrackDetailsDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? ISRC { get; set; }
    public int DurationSeconds { get; set; }

    public string? GenreName { get; set; }
    public string? MetadataLanguageName { get; set; }
    public string? AudioLanguageName { get; set; }

    public bool HasAudio { get; set; }
    public string? Codec { get; set; }
    public string? BitDepth { get; set; }
    public string? Channels { get; set; }
    public string? SampleRate { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }

    public List<TrackReleaseDto> Releases { get; set; } = new();

    public List<ContributionDto> Contributions { get; set; } = new();
}