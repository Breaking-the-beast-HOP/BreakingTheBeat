namespace Gateway.Contracts.Music.MusicRelease.Create
{
    public class CreateReleaseRequest
    {
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public Guid? GenreId { get; set; }
        public Guid? LanguageId { get; set; }

        public List<CreateReleaseVolumeRequest> Volumes { get; set; } = new();
        public List<CreateReleaseContributionRequest> Contributions { get; set; } = new();
    }

}
