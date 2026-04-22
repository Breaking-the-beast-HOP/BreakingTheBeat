namespace PodcastCatalog.Domain.Entities
{
    public class Podcast
    {
        private readonly List<Season> _seasons = new();
        private readonly List<Category> _categories = new();
        private readonly List<PodcastContribution> _hosts = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }

        public int LanguageId { get; private set; }
        public Language Language { get; private set; }

        public IReadOnlyList<Season> Seasons => _seasons.AsReadOnly();
        public IReadOnlyList<Category> Categories => _categories.AsReadOnly();
        public IReadOnlyList<PodcastContribution> Hosts => _hosts.AsReadOnly();

        public Podcast(string title, string description)
        {
            Id = Guid.NewGuid();
            Update(title, description);
        }
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
            LanguageId = int.Parse(language.Id.ToString());
        }

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (_categories.Any(c => c.Id == category.Id))
                return;

            _categories.Add(category);
        }

        public void AddHost(PodcastContribution host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (_hosts.Any(h => h.Id == host.Id))
                return;

            _hosts.Add(host);
        }
    }
}