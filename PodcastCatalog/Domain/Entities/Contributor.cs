namespace PodcastCatalog.Domain.Entities
{
    public class Contributor
    {

        public Contributor() { } // For EF Core
        public Guid Id { get; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public Contributor(string name) : this(name, null) { }
        public Contributor(string name, string description) : this(Guid.NewGuid(), name, description) { }
        public Contributor(Guid id, string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required");

            Id = id;
            Name = name;
            Description = description;
        }

        public void Update(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required");

            Name = name;
            Description = description;
        }
    }
}