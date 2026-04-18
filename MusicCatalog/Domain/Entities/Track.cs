
namespace MusicCatalog.Domain.Entities
{
    public class Track
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Version { get; private set; }
        public string? Isrc { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Language MetadataLanguage { get; private set; }
        public Language AudioLanguage { get; private set; } //just one audio for first version, can be expanded to multiple in future

        private readonly List<Contribution> _contributions = new();
        public IReadOnlyCollection<Contribution> Contributions => _contributions.AsReadOnly();
    }
}


           
   