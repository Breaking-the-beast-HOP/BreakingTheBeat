namespace PodcastCatalog.Domain.Entities
{
    public class Podcast
    {
        public Podcast() { } // For EF Core
        private readonly List<Season> _seasons = new();
        private readonly List<Contribution> _hosts = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }

        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; }

        public IReadOnlyList<Season> Seasons => _seasons.AsReadOnly();
        public IReadOnlyList<Contribution> Hosts => _hosts.AsReadOnly();

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public Podcast(string title, string description) : this(Guid.NewGuid(), title, description) { }
        public Podcast(Guid id, string title, string description)
        {
            Id = id;
            Update(title, description);
        }

        public void Update(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required");

            Title = title;
            Description = description;
        }

        // Aggregate behavior
        public void AddSeason(Season season)
        {
            if (season == null)
                throw new ArgumentNullException(nameof(season));
            if (_seasons.Any(s => s.Id == season.Id))
                return;

            _seasons.Add(season);
        }

        public void RemoveSeason(Guid seasonId)
        {
            _seasons.RemoveAll(s => s.Id == seasonId);
        }

        public void AddLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));
            Language = language;
            LanguageId = language.Id;
        }
        public void RemoveLanguage()
        {
            Language = null!;
            LanguageId = Guid.Empty;
        }

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            Category = category;
            CategoryId = category.Id;
        }

        public void AddHost(Contribution host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (_hosts.Any(h => h.Id == host.Id))
                return;

            _hosts.Add(host);
        }
    }
}