namespace PodcastCatalog.Domain.Entities
{
    public class Contribution
    {
        private Contribution() { } // For EF Core

        public Guid Id { get; private set; }
        public Guid ContributorId { get; private set; }
        public Guid ContributorRoleId { get; private set; }
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
            if ((podcast == null && episode == null) || (podcast != null && episode != null))
                throw new ArgumentException("A contribution must be associated with either a podcast or an episode, but not both.");

            Id = id;
            Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
            Role = role ?? throw new ArgumentNullException(nameof(role));
            Podcast = podcast;
            Episode = episode;

            ContributorId = contributor.Id;
            ContributorRoleId = role.Id;
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

        private Contribution(Guid contributorId, Guid contributorRoleId, Guid? episodeId, Guid? podcastId)
        {
            if (episodeId is null && podcastId is null)
                throw new ArgumentException("Contribution must belong to either a track or a release.");

            if (episodeId is not null && podcastId is not null)
                throw new ArgumentException("Contribution cannot belong to both track and release.");

            Id = Guid.NewGuid();

            ContributorId = contributorId;
            ContributorRoleId = contributorRoleId;
            EpisodeId = episodeId;
            PodcastId = podcastId;
        }

        public static Contribution ForEpisode(Guid episodeId, Guid contributorId, Guid roleId)
        {
            return new Contribution(contributorId, roleId, episodeId, null);
        }

        public static Contribution ForPodcast(Guid podcastId, Guid contributorId, Guid roleId)
        {
            return new Contribution(contributorId, roleId, null, podcastId);
        }

        public void ChangeRole(ContributorRole role)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
            ContributorRoleId = role.Id;
        }

        public void SetRights(bool hasRights)
        {
            HasRights = hasRights;
        }
    }
}