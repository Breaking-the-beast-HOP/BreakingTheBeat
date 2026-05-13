namespace PodcastCatalog.Domain.Entities
{
    public class Episode
    {
        public Guid Id { get; private set; }

        public string? Title { get; private set; }
        public string? Description { get; private set; }

        public TimeSpan? Duration { get; private set; }
        public DateTime? AiringDate { get; private set; }

        public int EpisodeNumber { get; private set; }

        public string Status { get; private set; } = "Draft";

        public Guid? TagId { get; private set; }
        public Tag? EpisodeTag { get; private set; }

        private readonly List<Contribution> _contributions = new();
        public IReadOnlyCollection<Contribution> Contributions => _contributions.AsReadOnly();

        public Guid SeasonId { get; private set; }
        public Season Season { get; private set; } = null!;

        private Episode() { } // EF Core

        public static Episode Create(
            Guid seasonId,
            string? title,
            string? description,
            TimeSpan? duration,
            DateTime? airingDate,
            Guid? tagId,
            int episodeNumber)
        {
            if (seasonId == Guid.Empty)
                throw new ArgumentException("SeasonId is required.");

            if (episodeNumber <= 0)
                throw new ArgumentException("Episode number must be greater than zero.");

            return new Episode
            {
                Id = Guid.NewGuid(),
                SeasonId = seasonId,
                Title = title,
                Description = description,
                Duration = duration,
                AiringDate = airingDate.HasValue
                            ? DateTime.SpecifyKind(airingDate.Value, DateTimeKind.Utc)
                            : null,
                TagId = tagId,
                EpisodeNumber = episodeNumber,
                Status = "Draft"
            };
        }

        public void UpdateDetails(
            string? title,
            string? description,
            TimeSpan? duration,
            DateTime? airingDate,
            Guid? tagId)
        {
            Title = title;
            Description = description;
            Duration = duration;
            AiringDate = airingDate.HasValue
                            ? DateTime.SpecifyKind(airingDate.Value, DateTimeKind.Utc)
                            : null;
            TagId = tagId;
        }

        public void MarkReadyForQc()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new InvalidOperationException("Title is required before sending episode to QC.");

            Status = "ReadyForQc";
        }

        public void MarkDraft()
        {
            Status = "Draft";
        }

        public void SetTag(Guid tagId)
        {
            if (tagId == Guid.Empty)
                throw new ArgumentException("TagId is required.");

            TagId = tagId;
        }

        public void RemoveTag()
        {
            TagId = null;
            EpisodeTag = null;
        }

        public void AddContribution(Guid contributorId, Guid roleId, bool hasRights)
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

            //var contribution = Contribution.ForEpisode(Id, contributorId, roleId);
            //contribution.SetRights(hasRights);

            //_contributions.Add(contribution);
        }

        public void RemoveContribution(Guid contributorId, Guid roleId)
        {
            _contributions.RemoveAll(c =>
                c.ContributorId == contributorId &&
                c.ContributorRoleId == roleId);
        }
    }
}