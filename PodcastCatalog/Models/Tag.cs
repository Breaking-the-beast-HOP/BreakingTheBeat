namespace PodcastCatalog.Models
{
    public class Tag
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

        public Tag CreateTag(int id, string name)
        {
            var tag = new Tag
            {
                Id = id,
                Name = name
            };
            return tag;
        }

        public Tag UpdateTag(Tag entity, string name)
        {
            entity.Name = name;
            return entity;
        }
    }
}