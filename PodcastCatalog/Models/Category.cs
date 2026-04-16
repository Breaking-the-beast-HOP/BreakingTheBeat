namespace PodcastCatalog.Models
{
    internal class Category
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

        public Category CreateCategory(int id, string name, string description)
        {
            var category = new Category
            {
                Id = id,
                Name = name,
                Description = description
            };
            return category;
        }

        public Category UpdateCategory(Category entity, string name, string description)
        {
            entity.Name = name;
            entity.Description = description;
            return entity;
        }
    }
}