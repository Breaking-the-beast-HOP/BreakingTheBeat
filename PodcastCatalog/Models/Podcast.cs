using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Models
{
    internal class Podcast
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _description;

        public string Description { get { return _description; } set { _description = value; } }

        private readonly List<Language> _languages;

        public IReadOnlyList<Language> MyProperty => _languages.AsReadOnly();

        private readonly List<Category> _categories;

        public IReadOnlyList<Category> Categories => _categories.AsReadOnly();

        public readonly List<Season> _seasons;

        public IReadOnlyList<Season> Seasons => _seasons.AsReadOnly();

        private readonly List<Contribution> _hosts;

        public IReadOnlyList<Contribution> Hosts => _hosts.AsReadOnly();

        public Podcast() { }
    }
}
