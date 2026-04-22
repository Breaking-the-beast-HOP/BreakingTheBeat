namespace PodcastCatalog.Domain.Entities
{
    public class Season
    {
        private readonly List<Episode> _episodes = new();

        public Guid Id { get; }
        public string Name { get; private set; }
        public DateOnly ProductionYear { get; private set; }

        public IReadOnlyList<Episode> Episodes => _episodes.AsReadOnly();

        // Database Relationships
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; } = null!;

        public Season(string name, DateOnly productionYear, int podcastId, Podcast podcast)
        {
            Id = Guid.NewGuid();
            Update(name, productionYear);
            PodcastId = podcastId;
            Podcast = podcast ?? throw new ArgumentNullException(nameof(podcast));
        }
        public Season(Guid id, string name, DateOnly productionYear, int podcastId, Podcast podcast)
        {
            Id = id;
            Update(name, productionYear);
            PodcastId = podcastId;
            Podcast = podcast ?? throw new ArgumentNullException(nameof(podcast));
        }

        public void Update(string name, DateOnly productionYear)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Season name required");

            Name = name;
            ProductionYear = productionYear;
        }

        // Aggregate behavior

        public Episode CreateEpisode(string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            var episode = new Episode(
                Guid.NewGuid(),
                title,
                description,
                duration,
                airingDate,
                int.Parse(Id.ToString()),
                this);

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