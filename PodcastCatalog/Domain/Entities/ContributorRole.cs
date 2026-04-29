namespace PodcastCatalog.Domain.Entities
{
    public class ContributorRole
    {
        public ContributorRole() { } // For EF Core
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public string? Description { get; private set; }
        public ContributorRole(string name, string description) : this(Guid.NewGuid(), name, description) { }
        public ContributorRole(Guid id, string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name required");

            Id = id;
            Name = name;
        }

        public void Update(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name required");
            Name = name;
            Description = description;
        }
    }
}