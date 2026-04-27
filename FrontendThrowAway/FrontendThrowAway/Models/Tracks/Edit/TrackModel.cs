namespace FrontendThrowAway.Models.Tracks.EditTrack
{
    public class TrackModel
    {
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? ISRC { get; set; }
        public Guid? GenreId { get; set; }
        public Guid? MetadataLanguageId { get; set; }
        public Guid? AudioLanguageId { get; set; }
        public List<UpdateContributionRequest> Contributions { get; set; } = new();
    }
}
