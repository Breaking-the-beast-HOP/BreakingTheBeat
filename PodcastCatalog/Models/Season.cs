namespace PodcastCatalog.Models
{
    public class Season
    {
        private readonly List<Episode> _episodes = new();

        public Guid Id { get; }
        public string Name { get; private set; }
        public DateOnly ProductionYear { get; private set; }

        public IReadOnlyList<Episode> Episodes => _episodes.AsReadOnly();

        public Season(Guid id, string name, DateOnly productionYear)
        {
            Id = id;
            Update(name, productionYear);
        }

        public void Update(string name, DateOnly productionYear)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Season name required");

            Name = name;
            ProductionYear = productionYear;
        }

        // 🔥 Aggregate behavior

        public Episode CreateEpisode(string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            var episode = new Episode(Guid.NewGuid(), title, description, duration, airingDate);

            _episodes.Add(episode);

            return episode;
        }

        public void AddEpisode(Episode episode)
        {
            if (episode == null)
                throw new ArgumentNullException(nameof(episode));

            if (_episodes.Any(e => e.Id == episode.Id))
                return;

            _episodes.Add(episode);
        }

        public void RemoveEpisode(Guid episodeId)
        {
            _episodes.RemoveAll(e => e.Id == episodeId);
        }

        public Episode GetEpisode(Guid episodeId)
        {
            return _episodes.FirstOrDefault(e => e.Id == episodeId);
        }
    }

}