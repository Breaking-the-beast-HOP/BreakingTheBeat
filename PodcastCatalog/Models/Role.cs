namespace PodcastCatalog.Models
{
    public class Role
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Role() : this(0, string.Empty) { }
        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Role CreateRole(int id, string name)
        {
            var role = new Role(id, name);
            return role;
        }

        public Role UpdateRole(Role entity, string name) { Name = name; return entity; }
    }
}