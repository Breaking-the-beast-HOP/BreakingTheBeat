namespace MusicCatalog.Domain.Entities
{
    public class Track
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Version { get; private set; }
        public string ISRC { get; private set; }
        public TimeSpan Duration { get; private set; }

        public Guid GenreId { get; private set; }
        public Genre Genre { get; private set; }

        public Guid MetadataLanguageId { get; private set; }
        public Language MetadataLanguage { get; private set; }

        public Guid AudioLanguageId { get; private set; }
        public Language AudioLanguage { get; private set; }

        private readonly List<Contribution> _contributions = new();
        public IReadOnlyCollection<Contribution> Contributions => _contributions.AsReadOnly();

        private Track() { }

        public static Track Create(string title, string? version, string isrc, Guid genreId, Guid audioLanguageId, Guid metadataLanguageId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            if (genreId == Guid.Empty)
                throw new ArgumentException("GenreId is required.");

            if (metadataLanguageId == Guid.Empty)
                throw new ArgumentException("MetadataLanguageId is required.");

            if (audioLanguageId == Guid.Empty)
                throw new ArgumentException("AudioLanguageId is required.");

            return new Track
            {
                Id = Guid.NewGuid(),
                Title = title,
                Version = version,
                ISRC = isrc,
                GenreId = genreId,
                MetadataLanguageId = metadataLanguageId,
                AudioLanguageId = audioLanguageId,
                Duration = TimeSpan.Zero
            };
        }

        public void UpdateDetails(string? title, string? version, string? isrc, Guid? genreId,Guid? metadataLanguageId,Guid? audioLanguageId)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Title = title;

            Version = version;

            if (!string.IsNullOrWhiteSpace(isrc))
                ISRC = isrc;

            if (genreId.HasValue)
            {
                if (genreId.Value == Guid.Empty)
                    throw new ArgumentException("GenreId is required.");

                GenreId = genreId.Value;
            }

            if (metadataLanguageId.HasValue)
            {
                if (metadataLanguageId.Value == Guid.Empty)
                    throw new ArgumentException("MetadataLanguageId is required.");

                MetadataLanguageId = metadataLanguageId.Value;
            }

            if (audioLanguageId.HasValue)
            {
                if (audioLanguageId.Value == Guid.Empty)
                    throw new ArgumentException("AudioLanguageId is required.");

                AudioLanguageId = audioLanguageId.Value;
            }
        }

        public void AddContribution(Guid contributorId, Guid roleId)
        {
            if (_contributions.Any(c =>
                c.ContributorId == contributorId &&
                c.ContributorRoleId == roleId))
            {
                return;
            }

            _contributions.Add(Contribution.ForTrack(Id, contributorId, roleId));
        }
    }
}




