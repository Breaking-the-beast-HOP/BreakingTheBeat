namespace Gateway.Contracts.Music.MusicRelease.Update
{
    public class UpdateReleaseTrackRequest
    {
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }
    }
}