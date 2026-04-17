using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodcastCatalog.Models;

namespace PodcastCatalog.Application
{
    public class ManagePodcasts
    {
        private readonly List<Podcast> _podcasts = new();
        private readonly List<Category> _categories = new();
        private readonly List<Language> _languages = new();

        public IReadOnlyList<Podcast> Podcasts => _podcasts.AsReadOnly();
        public IReadOnlyList<Category> Categories => _categories.AsReadOnly();
        public IReadOnlyList<Language> Languages => _languages.AsReadOnly();

        public ManagePodcasts()
        {
            // Populate Podcasts, Categories, and Languages with data from the database or any other source
        }

        public Podcast CreatePodcast(string title, string description)
        {
            var podcast = new Podcast(Guid.NewGuid(), title, description);
            _podcasts.Add(podcast);
            return podcast;
        }

    }
}
