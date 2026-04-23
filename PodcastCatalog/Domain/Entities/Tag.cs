namespace PodcastCatalog.Domain.Entities
{
    public class Tag
    {
        public Tag() { } // For EF Core
        public Guid Id { get; }
        public string Name { get; private set; } = null!;

        // Navigation property til EF Core
        public ICollection<Episode> Episodes { get; private set; } = new List<Episode>();

        public Tag(string name) : this(Guid.NewGuid(), name) { }

        public Tag(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tag name required");

            Id = id;
            Name = name;
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tag name required");

            Name = name;
        }

        public void AddEpisode(Episode episode)
        {
            if (episode == null) throw new ArgumentNullException(nameof(episode));
            if (Episodes.Any(e => e.Id == episode.Id)) return;
            Episodes.Add(episode);
        }
    }
}