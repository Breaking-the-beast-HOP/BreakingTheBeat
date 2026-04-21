namespace PodcastCatalog.Shared.Models
{
    public class Tag
    {
        public Guid Id { get; }
        public string Name { get; private set; } = null!;

        // Database Relationship
        public ICollection<Episode> Episodes { get; private set; } = new List<Episode>();

        public Tag(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tag name required");
            Id = Guid.NewGuid();
            Name = name;
        }
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
    }
}