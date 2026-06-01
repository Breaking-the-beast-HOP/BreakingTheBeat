using MediatR;
using MusicCatalogContext.Domain.Enums;
using MusicCatalogContext.Domain.Events;

namespace MusicCatalogContext.Domain.Entities
{
    public class Release
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; }
        public string? Version { get; private set; }
        public string? EAN { get; private set; }
        public DateTime? ReleaseDate { get; private set; }

        public ReleaseStatus Status { get; set; } = ReleaseStatus.Draft;

        public Guid GenreId { get; private set; }
        public Genre Genre { get; private set; }

        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; }

        private readonly List<ReleaseVolume> _volumes = new();
        public IReadOnlyCollection<ReleaseVolume> Volumes => _volumes.AsReadOnly();

        private readonly List<Contribution> _contributions = new();
        public IReadOnlyCollection<Contribution> Contributions => _contributions.AsReadOnly();

        //domain events
        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        private void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        private Release() { }

        public static Release Create(string title, string? version, string? ean, DateTime? releaseDate, Guid genreId, Guid languageId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            if (genreId == Guid.Empty)
                throw new ArgumentException("GenreId is required.");

            if (languageId == Guid.Empty)
                throw new ArgumentException("LanguageId is required.");

            return new Release
            {
                Id = Guid.NewGuid(),
                Title = title,
                Version = version,
                EAN = ean,
                ReleaseDate = releaseDate,
                GenreId = genreId,
                LanguageId = languageId,
            };
        }

        public ReleaseVolume AddVolume(int volumeNumber)
        {
            if (volumeNumber <= 0)
                throw new ArgumentException("Volume number must be greater than zero.");

            if (_volumes.Any(v => v.VolumeNumber == volumeNumber))
                throw new InvalidOperationException("Volume number must be unique.");

            var volume = ReleaseVolume.Create(Id, volumeNumber);
            _volumes.Add(volume);

            return volume;
        }

        public void UpdateVolumes(List<(int VolumeNumber, List<(Guid TrackId, int TrackNumber)> Tracks)> volumes)
        {
            if (volumes is null || !volumes.Any())
                throw new ArgumentException("At least one volume is required.");

            if (volumes.Any(v => v.VolumeNumber <= 0))
                throw new ArgumentException("Volume number must be greater than zero.");

            if (volumes.Select(v => v.VolumeNumber).Distinct().Count() != volumes.Count)
                throw new InvalidOperationException("Volume numbers must be unique.");

            if (volumes.Any(v => v.Tracks is null || !v.Tracks.Any()))
                throw new ArgumentException("Each volume must contain at least one track.");

            _volumes.RemoveAll(existing =>
                !volumes.Any(incoming => incoming.VolumeNumber == existing.VolumeNumber));

            foreach (var incomingVolume in volumes.OrderBy(v => v.VolumeNumber))
            {
                var existingVolume = _volumes
                    .FirstOrDefault(v => v.VolumeNumber == incomingVolume.VolumeNumber);

                if (existingVolume is null)
                {
                    existingVolume = ReleaseVolume.Create(Id, incomingVolume.VolumeNumber);
                    _volumes.Add(existingVolume);
                }

                existingVolume.UpdateTracks(incomingVolume.Tracks);
            }
        }



        public void AddContribution(Guid contributorId, Guid roleId)
        {
            if (contributorId == Guid.Empty)
                throw new ArgumentException("ContributorId is required.");

            if (roleId == Guid.Empty)
                throw new ArgumentException("RoleId is required.");

            if (_contributions.Any(c =>
                c.ContributorId == contributorId &&
                c.ContributorRoleId == roleId))
            {
                return;
            }

            _contributions.Add(Contribution.ForRelease(Id, contributorId, roleId));
        }


        public void UpdateDetails(string? title, string? version, string? ean, Guid? genreId, Guid? languageId)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Title = title;

            Version = version;

            if (!string.IsNullOrWhiteSpace(ean))
                EAN = ean;

            if (genreId.HasValue)
            {
                if (genreId.Value == Guid.Empty)
                    throw new ArgumentException("GenreId is required.");

                GenreId = genreId.Value;
            }

            if (languageId.HasValue)
            {
                if (languageId.Value == Guid.Empty)
                    throw new ArgumentException("MetadataLanguageId is required.");

                LanguageId = languageId.Value;
            }


        }

        public void UpdateContributions(List<(Guid ContributorId, Guid ContributorRoleId)> contributions)
        {
            // Fjern contributions som ikke længere findes i requesten
            _contributions.RemoveAll(existing =>
                !contributions.Any(incoming =>
                    incoming.ContributorId == existing.ContributorId &&
                    incoming.ContributorRoleId == existing.ContributorRoleId));

            // Tilføj nye contributions som ikke allerede findes
            foreach (var incoming in contributions)
            {
                var alreadyExists = _contributions.Any(existing =>
                    existing.ContributorId == incoming.ContributorId &&
                    existing.ContributorRoleId == incoming.ContributorRoleId);

                if (!alreadyExists)
                {
                    _contributions.Add(Contribution.ForRelease(
                        Id,
                        incoming.ContributorId,
                        incoming.ContributorRoleId));
                }
            }
        }

        public void SubmitToQc()
        {
            AddDomainEvent(new ReleaseSubmittedToQcEvent(Id, Title));
            Status = ReleaseStatus.ReadyForQc;
        }
    }
}