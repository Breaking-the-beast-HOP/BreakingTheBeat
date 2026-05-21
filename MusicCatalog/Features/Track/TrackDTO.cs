namespace MusicCatalogContext.Features.Track
{
    public class TrackDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;
        public string? Version { get; set; }
        public string ISRC { get; set; } = default!;
        public TimeSpan Duration { get; set; }

        // Genre
        public Guid GenreId { get; set; }
        public string GenreName { get; set; } = default!;

        // Metadata language
        public Guid MetadataLanguageId { get; set; }
        public string MetadataLanguageName { get; set; } = default!;

        // Audio language
        public Guid AudioLanguageId { get; set; }
        public string AudioLanguageName { get; set; } = default!;

        // Contributions
        public List<TrackContributionRowDTO> Contributions { get; set; } = new();


    }
}
