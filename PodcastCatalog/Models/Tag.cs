namespace PodcastCatalog.Models
{
    public class Tag
    {
        public Guid Id { get; }
        public string Name { get; private set; }

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