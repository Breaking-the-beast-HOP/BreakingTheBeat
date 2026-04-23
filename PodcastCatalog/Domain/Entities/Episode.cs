namespace PodcastCatalog.Domain.Entities
{
    public class Episode
    {
        // Parameterløs konstruktør til EF Core
        public Episode() { }

        private readonly List<Contribution> _contributions = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public DateOnly AiringDate { get; private set; }

        public Guid TagId { get; private set; }
        public Tag EpisodeTag { get; private set; }
        public IReadOnlyList<Contribution> Contributions => _contributions.AsReadOnly();

        // Database Relationships
        public Guid SeasonId { get; private set; }
        public Season Season { get; private set; } = null!;

        // Konstruktør uden Season-parameter
        public Episode(string title, string description, TimeSpan duration, DateOnly airingDate, Season season)
            : this(Guid.NewGuid(), title, description, duration, airingDate, season)
        {
        }

        // Primær konstruktør
        public Episode(Guid id, string title, string description, TimeSpan duration, DateOnly airingDate, Season season)
        {
            Id = id;
            Update(title, description, duration, airingDate);
            this.Season = season;
            SeasonId = season.Id;
        }

        public void Update(string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title required");

            Title = title;
            Description = description;
            Duration = duration;
            AiringDate = airingDate;
        }

        public void SetSeason(Season season)
        {
            Season = season ?? throw new ArgumentNullException(nameof(season));
            SeasonId = season.Id;
        }

        public void SetTag(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            EpisodeTag = tag;
            TagId = tag.Id;
        }

        public void RemoveTag(Guid tagId)
        {
            EpisodeTag = null!;
            TagId = Guid.Empty;
        }

        // Contributions
        public void AddContribution(Contributor contributor, ContributorRole role, bool hasRights)
        {
            if (_contributions.Any(c =>
                c.Contributor.Id == contributor.Id &&
                c.Role.Id == role.Id))
            {
                return;
            }

            _contributions.Add(new Contribution(
                contributor,
                role,
                this));
        }

        public void RemoveContribution(Guid contributorId, Guid roleId)
        {
            _contributions.RemoveAll(c =>
                c.Contributor.Id == contributorId &&
                c.Role.Id == roleId);
        }
    }
}