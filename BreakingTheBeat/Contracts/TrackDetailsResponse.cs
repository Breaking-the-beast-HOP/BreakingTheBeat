namespace Gateway.Contracts
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

        public bool HasAudio { get; set; }
        public string? Codec { get; set; }
        public string? BitDepth { get; set; }
        public string? Channels { get; set; }
        public string? SampleRate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        //public List<ContributorDto> Contributors { get; set; } = new();
        //public List<ReleaseDto> Releases { get; set; } = new();
    }

}
