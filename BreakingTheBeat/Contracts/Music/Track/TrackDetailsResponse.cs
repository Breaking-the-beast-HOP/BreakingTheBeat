using Gateway.Contracts.Music.Contribution;

namespace Gateway.Contracts.Music.Track
{
    public class TrackDetailsResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string? Version { get; set; }
        public string? ISRC { get; set; }
        public int DurationSeconds { get; set; }

        public string? GenreName { get; set; }
        public string? MetadataLanguageName { get; set; }
        public string? AudioLanguageName { get; set; }

        public Guid? GenreId { get; set; }
        public Guid? MetadataLanguageId { get; set; }
        public Guid? AudioLanguageId { get; set; }

        public bool HasAudio { get; set; }
        public string? Codec { get; set; }
        public string? BitDepth { get; set; }
        public string? Channels { get; set; }
        public string? SampleRate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public List<ContributionResponse> Contributions { get; set; } = new();
    }


}


