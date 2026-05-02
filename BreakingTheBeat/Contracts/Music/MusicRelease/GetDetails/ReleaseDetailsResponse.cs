namespace Gateway.Contracts.Music.MusicRelease.Get
{
    public class ReleaseDetailsResponse
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

        public List<ReleaseVolumeResponse> Volumes { get; set; } = new();
        public List<ReleaseContributionResponse> Contributions { get; set; } = new();
    }

    public class ReleaseVolumeResponse
    {
        public Guid Id { get; set; }
        public int VolumeNumber { get; set; }
        public List<ReleaseTrackResponse> Tracks { get; set; } = new();
    }

    public class ReleaseTrackResponse
    {
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }

        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? ISRC { get; set; }
        public int DurationSeconds { get; set; }
    }

    public class ReleaseContributionResponse
    {
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = "";
        public Guid ContributorRoleId { get; set; }
        public string ContributorRoleName { get; set; } = "";
    }
}