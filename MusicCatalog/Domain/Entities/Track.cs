namespace MusicCatalog.Domain.Entities
{
    public class Track
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Version { get; private set; }
        public string Isrc { get; private set; }
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

        public static Track Create( string title, string? version,string isrc,Guid genreId,Guid metadataLanguageId,Guid audioLanguageId)
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
                Isrc = isrc,
                GenreId = genreId,
                MetadataLanguageId = metadataLanguageId,
                AudioLanguageId = audioLanguageId,
                Duration = TimeSpan.Zero
            };
        }
    }
}




