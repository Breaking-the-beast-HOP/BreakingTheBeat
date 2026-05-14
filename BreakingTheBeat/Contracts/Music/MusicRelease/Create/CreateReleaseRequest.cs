namespace Gateway.Contracts.Music.MusicRelease.Create
{
    public class CreateReleaseRequest
    {
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Status { get; set; }

        public Guid? GenreId { get; set; }
        public Guid? LanguageId { get; set; }

        public List<CreateReleaseVolumeRequest> Volumes { get; set; } = new();
        public List<CreateReleaseContributionRequest> Contributions { get; set; } = new();
    }

    public class CreateReleaseVolumeRequest
    {
        public int VolumeNumber { get; set; }
        public List<CreateReleaseTrackRequest> Tracks { get; set; } = new();
    }

    public class CreateReleaseTrackRequest
    {
        public int TrackNumber { get; set; }
        public Guid TrackId { get; set; }
    }

    public class CreateReleaseContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}