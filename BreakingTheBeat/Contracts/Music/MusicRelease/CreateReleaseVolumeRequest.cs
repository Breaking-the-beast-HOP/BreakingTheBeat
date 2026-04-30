namespace Gateway.Contracts.Music.Release
{
    public class CreateReleaseVolumeRequest
    {
        public int VolumeNumber { get; set; }
        public List<CreateReleaseTrackRequest> Tracks { get; set; } = new();
    }
}