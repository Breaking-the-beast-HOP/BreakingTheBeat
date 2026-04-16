using System.Security.Principal;

namespace PodcastCatalog.Models
{
    public class Contribution
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _rights;

        public bool Rights
        {
            get { return _rights; }
            set { _rights = value; }
        }

        private Role _role;

        public Role Role
        {
            get { return _role; }
            set { _role = value; }
        }

        private Contributor _contributor;

        public Contributor Contributor
        {
            get { return _contributor; }
            set { _contributor = value; }
        }

        public Contribution() : this(0, false, new Role(), new Contributor()) { }
        public Contribution(int id, bool rights, Role role, Contributor contributor)
        {
            Id = id;
            Rights = rights;
            Role = role;
            Contributor = contributor;
        }
        public Contribution CreateContribution(int id, bool rights, Role role, Contributor contributor)
        {
            var contribution = new Contribution(id, rights, role, contributor);
            return contribution;
        }

        public Contribution UpdateContribution(Contribution entity, bool rights, Role role, Contributor contributor)
        {
            entity.Rights = rights;
            entity.Role = role;
            entity.Contributor = contributor;
            return entity;
        }
    }
}