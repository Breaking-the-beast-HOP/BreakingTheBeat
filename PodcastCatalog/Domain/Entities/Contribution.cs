namespace PodcastCatalog.Domain.Entities
{
    public class Contribution
    {
        private Contribution() { } // For EF Core

        public Guid Id { get; private set; }
        public Guid ContributorId { get; private set; }
        public Guid RoleId { get; private set; }
        public Guid? PodcastId { get; private set; }
        public Guid? EpisodeId { get; private set; }

        public bool HasRights { get; private set; }
        public ContributorRole Role { get; private set; }
        public Contributor Contributor { get; private set; }
        public Podcast? Podcast { get; private set; }
        public Episode? Episode { get; private set; }

        // Primær konstruktør (bruges af de andre konstruktører)
        private Contribution(Guid id, Contributor contributor, ContributorRole role, Podcast? podcast, Episode? episode)
        {
            Id = id;
            Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
            Role = role ?? throw new ArgumentNullException(nameof(role));
            Podcast = podcast;
            Episode = episode;

            ContributorId = contributor.Id;
            RoleId = role.Id;
            PodcastId = podcast?.Id;
            EpisodeId = episode?.Id;
        }
        public Contribution(Contributor contributor, ContributorRole role, Podcast podcast)
            : this(Guid.NewGuid(), contributor, role, podcast, null)
        {
        }
        public Contribution(Guid id, Contributor contributor, ContributorRole role, Podcast podcast)
            : this(id, contributor, role, podcast, null)
        {
        }
        public Contribution(Contributor contributor, ContributorRole role, Episode episode)
            : this(Guid.NewGuid(), contributor, role, null, episode)
        {
        }
        public Contribution(Guid id, Contributor contributor, ContributorRole role, Episode episode)
            : this(id, contributor, role, null, episode)
        {
        }

        public void ChangeRole(ContributorRole role)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
            RoleId = role.Id;
        }

        public void SetRights(bool hasRights)
        {
            HasRights = hasRights;
        }
    }
}