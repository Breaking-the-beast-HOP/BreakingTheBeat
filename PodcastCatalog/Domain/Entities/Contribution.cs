namespace PodcastCatalog.Domain.Entities
{
    public class Contribution
    {
        private Contribution() { } // EF Core

        public Guid Id { get; private set; }

        public Guid ContributorId { get; private set; }
        public Guid ContributorRoleId { get; private set; }

        public Guid? PodcastId { get; private set; }
        public Guid? EpisodeId { get; private set; }

        public Contributor Contributor { get; private set; } = null!;
        public ContributorRole Role { get; private set; } = null!;
        public Podcast? Podcast { get; private set; }
        public Episode? Episode { get; private set; }

        private Contribution(
            Guid contributorId,
            Guid contributorRoleId,
            Guid? podcastId,
            Guid? episodeId)
        {
            if (contributorId == Guid.Empty)
                throw new ArgumentException("ContributorId is required.");

            if (contributorRoleId == Guid.Empty)
                throw new ArgumentException("ContributorRoleId is required.");

            if (podcastId is null && episodeId is null)
                throw new ArgumentException("Contribution must belong to either a podcast or an episode.");

            if (podcastId is not null && episodeId is not null)
                throw new ArgumentException("Contribution cannot belong to both podcast and episode.");

            Id = Guid.NewGuid();
            ContributorId = contributorId;
            ContributorRoleId = contributorRoleId;
            PodcastId = podcastId;
            EpisodeId = episodeId;
        }

        public static Contribution ForPodcast(Guid podcastId, Guid contributorId, Guid roleId)
        {
            return new Contribution(
                contributorId,
                roleId,
                podcastId,
                null);
        }

        public static Contribution ForEpisode(Guid episodeId, Guid contributorId, Guid roleId)
        {
            return new Contribution(
                contributorId,
                roleId,
                null,
                episodeId);
        }

    }
}