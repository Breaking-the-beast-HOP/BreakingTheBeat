namespace PodcastCatalog.Domain.Entities
{
    public class Podcast
    {
        public Podcast() { } // For EF Core
        private readonly List<Season> _seasons = new();
        private readonly List<Contribution> _contributions = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }

        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; }

        public IReadOnlyList<Season> Seasons => _seasons.AsReadOnly();
        public IReadOnlyList<Contribution> Contributions => _contributions.AsReadOnly();

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public Podcast(string title, string description) : this(Guid.NewGuid(), title, description, Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111")) { }
        public Podcast(Guid id, string title, string description, Guid categoryId, Guid languageId)
        {
            Id = id;
            CategoryId = categoryId;
            LanguageId = languageId;
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

        public Podcast ChangeLanguage(Guid languageId)
        {
            LanguageId = languageId;
            return this;
        }

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            Category = category;
            CategoryId = category.Id;
        }

        public void AddContribution(Contribution host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (_contributions.Any(h => h.Id == host.Id))
                return;

            _contributions.Add(host);
        }
    }
}