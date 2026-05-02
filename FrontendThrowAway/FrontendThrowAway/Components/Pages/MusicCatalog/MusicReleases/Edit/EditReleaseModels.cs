namespace FrontendThrowAway.Components.Pages.MusicCatalog.MusicReleases.Edit
{
    public class ReleaseModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public Guid? GenreId { get; set; }
        public Guid? LanguageId { get; set; }

        public List<ReleaseVolumeModel> Volumes { get; set; } = new();
        public List<CreateContributionRequest> Contributions { get; set; } = new();
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

    public class CreateContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }

    public class ReleaseDetailsDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public Guid? GenreId { get; set; }
        public Guid? LanguageId { get; set; }

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
    }

    public class TrackDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string? Version { get; set; }
        public string? ISRC { get; set; }
        public int DurationSeconds { get; set; }
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

    public class LanguageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string ISO { get; set; } = "";
    }

    public class GenreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
    }

}
