namespace Gateway.Contracts
{
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

        public class ContributionResponse
        {
            public Guid ContributorId { get; set; }
            public string ContributorName { get; set; } = "";
            public List<ContributionRoleResponse> Roles { get; set; } = new();
        }

        public class ContributionRoleResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = "";
        }
    }

}
