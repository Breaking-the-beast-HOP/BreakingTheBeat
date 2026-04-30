namespace Gateway.Contracts.Music.Release
{
    public class CreateReleaseTrackRequest
    {
        public int TrackNumber { get; set; }
        public Guid TrackId { get; set; }
    }
}
