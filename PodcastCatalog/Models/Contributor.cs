namespace PodcastCatalog.Models
{
    public class Contributor
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

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Contributor() : this(0, string.Empty, string.Empty) { }
        public Contributor(int id, string name, string description)
        {
            _id = id;
            _name = name;
            _description = description;
        }

        public Contributor CreateContributor(int id, string name, string description)
        {
            return new Contributor(id, name, description);
        }

        public Contributor UpdateContributor(Contributor entity, string name, string description)
        {
            entity.Name = name;
            entity.Description = description;
            return entity;
        }
    }
}