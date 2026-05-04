namespace MusicCatalog.Domain.Entities
{
    public class Release
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; }
        public string? Version { get; private set; }
        public string? EAN { get; private set; }
        public DateTime? ReleaseDate { get; private set; }

        public Guid GenreId { get; private set; }
        public Genre Genre { get; private set; }

        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; }

        private readonly List<ReleaseVolume> _volumes = new();
        public IReadOnlyCollection<ReleaseVolume> Volumes => _volumes.AsReadOnly();

        private readonly List<Contribution> _contributions = new();
        public IReadOnlyCollection<Contribution> Contributions => _contributions.AsReadOnly();

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
                LanguageId = languageId
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
    }
}