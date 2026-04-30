namespace MusicCatalog.Domain.Entities
{
    public class VolumeTrack
    {
        public Guid Id { get; private set; }

        public Guid ReleaseVolumeId { get; private set; }
        public ReleaseVolume ReleaseVolume { get; private set; }

        public Guid TrackId { get; private set; }
        public Track Track { get; private set; }

        public int TrackNumber { get; private set; }

        private VolumeTrack() { }

        internal static VolumeTrack Create( Guid releaseVolumeId,Guid trackId, int trackNumber)
        {
            if (releaseVolumeId == Guid.Empty)
                throw new ArgumentException("ReleaseVolumeId is required.");

            if (trackId == Guid.Empty)
                throw new ArgumentException("TrackId is required.");

            if (trackNumber <= 0)
                throw new ArgumentException("Track number must be greater than zero.");

            return new VolumeTrack
            {
                Id = Guid.NewGuid(),
                ReleaseVolumeId = releaseVolumeId,
                TrackId = trackId,
                TrackNumber = trackNumber
            };
        }
    }
}