using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Models
{
    internal class Language
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

        private string _iso;

        public string ISO
        {
            get { return _iso; }
            set { _iso = value; }
        }

        public Language CreateLanguage(int id, string name, string iso)
        {
            var language = new Language
            {
                Id = id,
                Name = name,
                ISO = iso
            };
            return language;
        }

        public Language UpdateLanguage(Language entity, string name, string iso)
        {
            entity.Name = name;
            entity.ISO = iso;
            return entity;
        }
    }
}
