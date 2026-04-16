namespace MusicCatalog.Domain.Entities
{
    public class Track
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Version { get; private set; }
        public string Isrc { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Language MetadataLanguage { get; private set; }
        private readonly List<Language> _audioLanguages = new();

        private readonly List<Contribution> _contributions = new();
        public DateTime CreatedAt { get; private set; }
        public int CreatedBy { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }
        public int? LastUpdatedBy { get; private set; }

    }
}
