namespace Gateway.Contracts.Music.MusicRelease.Update
{
    public class UpdateReleaseRequest
    {
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public Guid? GenreId { get; set; }
        public Guid? LanguageId { get; set; }

        public List<UpdateReleaseContributionRequest> Contributions { get; set; } = new();
        public List<UpdateReleaseVolumeRequest> Volumes { get; set; } = new();
    }

    public class UpdateReleaseVolumeRequest
    {
        public int VolumeNumber { get; set; }
        public List<UpdateReleaseTrackRequest> Tracks { get; set; } = new();
    }

    public class UpdateReleaseTrackRequest
    {
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }
    }

    public class UpdateReleaseContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}