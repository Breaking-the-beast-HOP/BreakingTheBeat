using System.Security.Principal;

namespace PodcastCatalog.Models
{
    public class Contribution
    {
        public Guid Id { get; }
        public bool HasRights { get; private set; }
        public Role Role { get; private set; }
        public Contributor Contributor { get; }

        public Contribution(Guid id, Contributor contributor, Role role, bool hasRights)
        {
            Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
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