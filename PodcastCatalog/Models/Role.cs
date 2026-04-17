namespace PodcastCatalog.Models
{
    public class Role
    {
        public Guid Id { get; }
        public string Name { get; }

        public Role(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name required");

            Id = id;
            Name = name;
        }
    }
}