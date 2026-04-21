namespace PodcastCatalog.Shared.Models
{
    public class Category
    {
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
        private Category(Guid id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public static Category Create(Guid id, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required");

            return new Category(id, name, description);
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