namespace PodcastCatalog.Domain.Entities
{
    public class Language
    {
        public Language() { } // For EF Core
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ISO { get; private set; }

        public Language(string name, string iso) : this(Guid.NewGuid(), name, iso) { }
        public Language(Guid id, string name, string iso)
        {
            Id = id;
            Name = name;
            ISO = iso;
        }

        public void Update(string name, string iso)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Language name is required");
            if (string.IsNullOrWhiteSpace(iso))
                throw new ArgumentException("ISO code is required");

            Name = name;
            ISO = iso;
        }
    }
}