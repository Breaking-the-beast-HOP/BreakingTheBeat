namespace PodcastCatalog.Domain.Entities
{
    public class Season
    {
        public Season() { } // For EF Core

        private readonly List<Episode> _episodes = new();
        public Guid Id { get; private set; } // Ændret fra `public Guid Id { get; }` til `private set` for at tillade EF Core at sætte Id
        public string Name { get; private set; }
        public DateOnly ProductionYear { get; private set; }

        public IReadOnlyList<Episode> Episodes => _episodes.AsReadOnly();

        // Database Relationships
        public Guid PodcastId { get; private set; } // Ændret fra `public int PodcastId { get; set; }` til `private set`
        public Podcast Podcast { get; private set; } = null!;

        public Season(string name, DateOnly productionYear, Guid podcastId, Podcast podcast) :
            this(Guid.NewGuid(), name, productionYear, podcastId, podcast)
        { }

        public Season(Guid id, string name, DateOnly productionYear, Guid podcastId, Podcast podcast)
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

        // Opdateret CreateEpisode-metode
        public Episode CreateEpisode(string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            var episode = new Episode(
                title,
                description,
                duration,
                airingDate,
                this);

            episode.SetSeason(this); // Sæt Season manuelt
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