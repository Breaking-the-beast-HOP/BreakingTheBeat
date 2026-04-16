namespace PodcastCatalog.Models
{
    public class Season
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

        private DateOnly _productionYear;

        public DateOnly ProductionYear
        {
            get { return _productionYear; }
            set { _productionYear = value; }
        }

        private readonly List<Episode> _episodes;

        public IReadOnlyList<Episode> Episodes => _episodes.AsReadOnly();

        private readonly List<Contribution> _guests;

        public IReadOnlyList<Contribution> Guests => _guests.AsReadOnly();

        public Season()
        {
            _episodes = new List<Episode>();
        }

        public Season CreateSeason(int id, string name, DateOnly productionYear)
        {
            return new Season
            {
                Id = id,
                Name = name,
                ProductionYear = productionYear
            };
        }

        public Season UpdateSeason(Season entity, string name, DateOnly productionYear)
        {
            entity.Name = name;
            entity.ProductionYear = productionYear;
            return entity;
        }

        public void AddEpisode(Episode episode)
        {
            _episodes.Add(episode);
        }

        public void AddEpisodes(IEnumerable<Episode> episodes)
        {
            _episodes.AddRange(episodes);
        }

        public void UpdateEpisode(Episode episode)
        {
            var existingEpisode = _episodes.FirstOrDefault(e => e.Id == episode.Id);
            if (existingEpisode != null)
            {
                existingEpisode.UpdateEpisode(existingEpisode, episode.Title, episode.Description, episode.Duration, episode.AiringDate);
            }
        }
        public void RemoveEpisode(Episode episode)
        {
            _episodes.Remove(episode);
        }
    }
}