using System.Security.Principal;

namespace PodcastCatalog.Domain.Entities
{
    public class PodcastContribution
    {
        public Guid Id { get; }
        public bool HasRights { get; private set; }

        // Database Relationships
        public int PodcastId { get; private set; }
        public Podcast Podcast { get; private set; }

        public int RoleId { get; private set; }
        public Role Role { get; private set; }

        public int ContributorId { get; private set; }
        public Contributor Contributor { get; private set; }

        public PodcastContribution(int podcastId,
                                   Podcast podcast,
                                   int contributorId,
                                   Contributor contributor,
                                   int roleId,
                                   Role role,
                                   bool hasRights)
        {
            PodcastId = podcastId;
            Podcast = podcast ?? throw new ArgumentNullException(nameof(podcast));
            ContributorId = contributorId;
            Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
            RoleId = roleId;
            Role = role ?? throw new ArgumentNullException(nameof(role));

            Id = Guid.NewGuid();
            HasRights = hasRights;
        }
        public PodcastContribution(Guid id,
                                   int podcastId,
                                   Podcast podcast,
                                   int contributorId,
                                   Contributor contributor,
                                   int roleId,
                                   Role role,
                                   bool hasRights)
        {
            PodcastId = podcastId;
            Podcast = podcast ?? throw new ArgumentNullException(nameof(podcast));
            ContributorId = contributorId;
            Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
            RoleId = roleId;
            Role = role ?? throw new ArgumentNullException(nameof(role));

            Id = id;
            HasRights = hasRights;
        }

        public void ChangeRole(Role role)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }

        public void SetRights(bool hasRights)
        {
            HasRights = hasRights;
        }
    }
}