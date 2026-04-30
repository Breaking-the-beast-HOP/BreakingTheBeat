namespace Gateway.Contracts
{
    public class ReleaseTrackResponse
    {
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }

        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? ISRC { get; set; }
        public int DurationSeconds { get; set; }
    }
}
