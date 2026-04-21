namespace PodcastCatalog.Shared.Models
{
    public class Role
    {
        public Guid Id { get; }
        public string Name { get; } = null!;

        public Role(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name required");
            Name = name;
            Id = Guid.NewGuid();
        }
        public Role(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name required");

            Id = id;
            Name = name;
        }
    }
}