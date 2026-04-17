namespace PodcastCatalog.Models
{
    public class Language
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ISO { get; private set; }

        private Language(int id, string name, string iso)
        {
            Id = id;
            Name = name;
            ISO = iso;
        }

        public static Language Create(int id, string name, string iso)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Language name is required");
            if (string.IsNullOrWhiteSpace(iso))
                throw new ArgumentException("ISO code is required");

            return new Language(id, name, iso);
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