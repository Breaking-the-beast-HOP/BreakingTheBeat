namespace FrontendThrowAway.Models.Tracks.EditTrack
{
    public class TrackEditDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? ISRC { get; set; }
        public Guid? GenreId { get; set; }
        public Guid? MetadataLanguageId { get; set; }
        public Guid? AudioLanguageId { get; set; }

        public List<ContributionDto> Contributions { get; set; } = new();
    }
}
