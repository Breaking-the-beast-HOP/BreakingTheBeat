using Gateway.Contracts.Music.Contribution;

namespace Gateway.Contracts.Music.Release
{
    public class ReleaseDetailsResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public string? GenreName { get; set; }
        public string? LanguageName { get; set; }

        public bool HasCover { get; set; }
        public bool HasBooklet { get; set; }

        public List<ReleaseVolumeResponse> Volumes { get; set; } = new();
        public List<ContributionResponse> Contributions { get; set; } = new();
    }
}