namespace MusicCatalog.Domain.Entities
{
    public class ReleaseVolume
    {
        public Guid Id { get; private set; }

        public Guid ReleaseId { get; private set; }
        public Release Release { get; private set; }

        public int VolumeNumber { get; private set; }

        private readonly List<ReleaseTrack> _tracks = new();
        public IReadOnlyCollection<ReleaseTrack> Tracks => _tracks.AsReadOnly();

        private ReleaseVolume() { }

        internal static ReleaseVolume Create(Guid releaseId, int volumeNumber)
        {
            if (releaseId == Guid.Empty)
                throw new ArgumentException("ReleaseId is required.");

            if (volumeNumber <= 0)
                throw new ArgumentException("Volume number must be greater than zero.");

            return new ReleaseVolume
            {
                Id = Guid.NewGuid(),
                ReleaseId = releaseId,
                VolumeNumber = volumeNumber
            };
        }

        public void AddTrack(Guid trackId, int trackNumber)
        {
            if (trackId == Guid.Empty)
                throw new ArgumentException("TrackId is required.");

            if (trackNumber <= 0)
                throw new ArgumentException("Track number must be greater than zero.");

            if (_tracks.Any(t => t.TrackNumber == trackNumber))
                throw new InvalidOperationException("Track number must be unique within volume.");

            if (_tracks.Any(t => t.TrackId == trackId))
                throw new InvalidOperationException("Track already exists in this volume.");

            _tracks.Add(ReleaseTrack.Create(Id, trackId, trackNumber));
        }
    }
}