namespace Gateway.Contracts
{
    public class CreateReleaseVolumeRequest
    {
        public int VolumeNumber { get; set; }
        public List<CreateReleaseTrackRequest> Tracks { get; set; } = new();
    }
}