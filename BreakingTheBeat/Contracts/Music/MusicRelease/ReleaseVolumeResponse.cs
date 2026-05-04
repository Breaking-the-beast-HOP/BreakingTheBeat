namespace Gateway.Contracts.Music.Release
{
    public class ReleaseVolumeResponse
    {
        public Guid Id { get; set; }
        public int VolumeNumber { get; set; }
        public List<ReleaseTrackResponse> Tracks { get; set; } = new();
    }

}
