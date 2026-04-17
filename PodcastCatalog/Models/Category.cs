namespace PodcastCatalog.Models
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        private Category(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public static Category Create(int id, string name, string description)
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