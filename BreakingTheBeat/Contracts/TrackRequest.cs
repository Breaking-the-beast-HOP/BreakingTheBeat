namespace Gateway.Contracts
{
    public class TrackRequest
    {
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? ISRC { get; set; }
        public Guid? GenreId { get; set; }
        public Guid LanguageId { get; set; }
    }
}
