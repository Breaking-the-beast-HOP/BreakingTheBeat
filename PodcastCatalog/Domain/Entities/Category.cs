namespace PodcastCatalog.Domain.Entities
{
    public class Category
    {
        public Category() { } // For EF Core
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        // Navigation property
        public ICollection<Podcast> Podcasts { get; private set; } = new List<Podcast>();

        public Category(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }
        public Category(Guid id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public void Update(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required");

            Name = name;
            Description = description;
        }
    }
}