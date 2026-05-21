namespace MusicCatalogContext.Domain.Entities
{
    public class Country
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ISO { get; private set; }

        private Country() { } // EF Core

        public Country(Guid id, string name, string iso)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Country name cannot be empty");

            if (string.IsNullOrWhiteSpace(iso))
                throw new ArgumentException("ISO code cannot be empty");

            if (iso.Length > 10)
                throw new ArgumentException("ISO code too long");

            Id = id;
            Name = name;
            ISO = iso;
        }
    }
}