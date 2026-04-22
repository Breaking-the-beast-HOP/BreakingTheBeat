using System.Data;
using System.Security.Principal;

namespace PodcastCatalog.Domain.Entities
{
    public class EpisodeContribution
    {
        public Guid Id { get; }
        public bool HasRights { get; private set; }

        public int EpisodeId { get; set; }
        private Episode Episode { get; set; } = null!;

        public int RoleId { get; private set; }
        public Role Role { get; private set; }
        public int ContributorId { get; private set; }
        public Contributor Contributor { get; private set; }

        public EpisodeContribution(int episodeId,
            Episode episode,
            int contributorId,
            Contributor contributor,
            int roleId,
            Role role,
            bool hasRights)
        {
            Id = Guid.NewGuid();
            EpisodeId = episodeId;
            Episode = episode ?? throw new ArgumentNullException(nameof(episode));
            ContributorId = contributorId;
            Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
            Role = role ?? throw new ArgumentNullException(nameof(role));
            RoleId = roleId;
            HasRights = hasRights;
        }
        public EpisodeContribution(Guid id,
                                   int episodeId,
                                   Episode episode,
                                   int contributorId,
                                   Contributor contributor,
                                   int roleId,
                                   Role role,
                                   bool hasRights)
        {
            EpisodeId = episodeId;
            Episode = episode ?? throw new ArgumentNullException(nameof(episode));
            ContributorId = contributorId;
            Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
            Role = role ?? throw new ArgumentNullException(nameof(role));
            RoleId = roleId;

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