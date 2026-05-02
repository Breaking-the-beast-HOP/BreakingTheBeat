namespace MusicCatalog.Domain.Entities
{
    public class ReleaseVolume
    {
        public Guid Id { get; private set; }

        public Guid ReleaseId { get; private set; }
        public Release Release { get; private set; }

        public int VolumeNumber { get; private set; }

        private readonly List<VolumeTrack> _tracks = new();
        public IReadOnlyCollection<VolumeTrack> Tracks => _tracks.AsReadOnly();

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

            _tracks.Add(VolumeTrack.Create(Id, trackId, trackNumber));
        }

        public void UpdateTracks(List<(Guid TrackId, int TrackNumber)> tracks)
        {
            if (tracks is null || !tracks.Any())
                throw new ArgumentException("A volume must contain at least one track.");

            if (tracks.Any(t => t.TrackId == Guid.Empty))
                throw new ArgumentException("TrackId is required.");

            if (tracks.Any(t => t.TrackNumber <= 0))
                throw new ArgumentException("Track number must be greater than zero.");

            if (tracks.Select(t => t.TrackNumber).Distinct().Count() != tracks.Count)
                throw new InvalidOperationException("Track numbers must be unique within volume.");

            _tracks.RemoveAll(existing =>
                !tracks.Any(incoming =>
                    incoming.TrackId == existing.TrackId &&
                    incoming.TrackNumber == existing.TrackNumber));

            foreach (var incoming in tracks.OrderBy(t => t.TrackNumber))
            {
                var alreadyExists = _tracks.Any(existing =>
                    existing.TrackId == incoming.TrackId &&
                    existing.TrackNumber == incoming.TrackNumber);

                if (!alreadyExists)
                {
                    _tracks.Add(VolumeTrack.Create(
                        Id,
                        incoming.TrackId,
                        incoming.TrackNumber));
                }
            }
        }
    }
}