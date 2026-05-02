namespace Gateway.Contracts.Music.MusicRelease.Update
{
    public class UpdateVolumeRequest
    {
        public int VolumeNumber { get; set; }
        public List<UpdateReleaseTrackRequest> Tracks { get; set; } = new();
    }
}
