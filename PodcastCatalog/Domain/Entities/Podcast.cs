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
            List<(Guid ContributorId, Guid ContributorRoleId, bool HasRights)> incoming)
        {
            incoming ??= new();

            // 1. REMOVE (tracked entities only)
            var toRemove = _contributions
                .Where(existing => !incoming.Any(i =>
                    i.ContributorId == existing.ContributorId &&
                    i.ContributorRoleId == existing.ContributorRoleId))
                .ToList();

            foreach (var r in toRemove)
                _contributions.Remove(r);

            // 2. UPDATE EXISTING
            foreach (var existing in _contributions)
            {
                var match = incoming.FirstOrDefault(i =>
                    i.ContributorId == existing.ContributorId &&
                    i.ContributorRoleId == existing.ContributorRoleId);

                if (match != default)
                    existing.SetRights(match.HasRights);
            }

            // 3. ADD NEW (IMPORTANT: DO NOT use detached contributor objects)
            var existingKeys = _contributions
                .Select(c => (c.ContributorId, c.ContributorRoleId))
                .ToHashSet();

            foreach (var i in incoming)
            {
                if (existingKeys.Contains((i.ContributorId, i.ContributorRoleId)))
                    continue;

                var contribution = Contribution.ForPodcast(Id, i.ContributorId, i.ContributorRoleId);
                contribution.SetRights(i.HasRights);

                _contributions.Add(contribution);
            }
        }
    }
}