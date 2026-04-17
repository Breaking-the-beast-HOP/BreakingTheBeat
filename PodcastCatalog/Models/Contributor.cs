namespace PodcastCatalog.Models
{
    public class Contributor
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Contributor(Guid id, string name, string description)
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