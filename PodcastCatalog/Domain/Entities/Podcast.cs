namespace PodcastCatalog.Domain.Entities
{
    public class Podcast
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; } = null!;

        private readonly List<Season> _seasons = new();
        public IReadOnlyCollection<Season> Seasons => _seasons.AsReadOnly();

        private readonly List<Contribution> _contributions = new();
        public IReadOnlyCollection<Contribution> Contributions => _contributions.AsReadOnly();

        private Podcast() { } // EF Core

        public static Podcast Create(
            string title,
            string? description,
            Guid categoryId,
            Guid languageId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            if (categoryId == Guid.Empty)
                throw new ArgumentException("CategoryId is required.");

            if (languageId == Guid.Empty)
                throw new ArgumentException("LanguageId is required.");

            return new Podcast
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                CategoryId = categoryId,
                LanguageId = languageId
            };
        }

        public Season AddSeason(string? title, int seasonNumber)
        {
            if (seasonNumber <= 0)
                throw new ArgumentException("Season number must be greater than zero.");

            if (_seasons.Any(s => s.SeasonNumber == seasonNumber))
                throw new InvalidOperationException("Season number must be unique.");

            var season = Season.Create(Id, title, seasonNumber);
            _seasons.Add(season);

            return season;
        }

        public void AddSeason(Season season)
        {
            if (season is null)
                throw new ArgumentNullException(nameof(season));

            if (_seasons.Any(s => s.Id == season.Id))
                return;

            if (_seasons.Any(s => s.SeasonNumber == season.SeasonNumber))
                throw new InvalidOperationException("Season number must be unique.");

            _seasons.Add(season);
        }

        public void RemoveSeason(Guid seasonId)
        {
            _seasons.RemoveAll(s => s.Id == seasonId);
        }

        public void UpdateDetails(
            string? title,
            string? description,
            Guid? categoryId,
            Guid? languageId)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Title = title;

            Description = description;

            if (categoryId.HasValue)
            {
                if (categoryId.Value == Guid.Empty)
                    throw new ArgumentException("CategoryId is required.");

                CategoryId = categoryId.Value;
            }

            if (languageId.HasValue)
            {
                if (languageId.Value == Guid.Empty)
                    throw new ArgumentException("LanguageId is required.");

                LanguageId = languageId.Value;
            }
        }

        public void AddContribution(Guid contributorId, Guid roleId, bool hasRights = false)
        {
            if (contributorId == Guid.Empty)
                throw new ArgumentException("ContributorId is required.");

            if (roleId == Guid.Empty)
                throw new ArgumentException("RoleId is required.");

            if (_contributions.Any(c =>
                c.ContributorId == contributorId &&
                c.ContributorRoleId == roleId))
            {
                return;
            }

            var contribution = Contribution.ForPodcast(Id, contributorId, roleId);
            contribution.SetRights(hasRights);

            _contributions.Add(contribution);
        }

        public void UpdateContributions(
            List<(Guid ContributorId, Guid ContributorRoleId, bool HasRights)> contributions)
        {
            contributions ??= new();

            _contributions.RemoveAll(existing =>
                !contributions.Any(incoming =>
                    incoming.ContributorId == existing.ContributorId &&
                    incoming.ContributorRoleId == existing.ContributorRoleId));

            foreach (var incoming in contributions)
            {
                if (incoming.ContributorId == Guid.Empty)
                    throw new ArgumentException("ContributorId is required.");

                if (incoming.ContributorRoleId == Guid.Empty)
                    throw new ArgumentException("ContributorRoleId is required.");

                var existing = _contributions.FirstOrDefault(c =>
                    c.ContributorId == incoming.ContributorId &&
                    c.ContributorRoleId == incoming.ContributorRoleId);

                if (existing is not null)
                {
                    existing.SetRights(incoming.HasRights);
                    continue;
                }

                var contribution = Contribution.ForPodcast(
                    Id,
                    incoming.ContributorId,
                    incoming.ContributorRoleId);

                contribution.SetRights(incoming.HasRights);

                _contributions.Add(contribution);
            }
        }
    }
}