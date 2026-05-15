namespace PodcastCatalog.Domain.Entities
{
    public class Season
    {
        private readonly List<Episode> _episodes = new();

        public Guid Id { get; private set; }

        public string? Title { get; private set; }
        public int SeasonNumber { get; private set; }
        public DateOnly? ReleaseDate { get; private set; }

        public IReadOnlyCollection<Episode> Episodes => _episodes.AsReadOnly();

        public Guid PodcastId { get; private set; }
        public Podcast Podcast { get; private set; } = null!;

        private Season() { } // EF Core

        public static Season Create(Guid podcastId, string? title, int seasonNumber, DateOnly? releaseDate = null)
        {
            if (podcastId == Guid.Empty)
                throw new ArgumentException("PodcastId is required.");

            if (seasonNumber <= 0)
                throw new ArgumentException("Season number must be greater than zero.");

            if (releaseDate == null)
                releaseDate = DateOnly.FromDateTime(DateTime.UtcNow);

            return new Season
            {
                Id = Guid.NewGuid(),
                PodcastId = podcastId,
                Title = title,
                SeasonNumber = seasonNumber,
                ReleaseDate = releaseDate
            };
        }

        public Episode AddEpisode(
            string? title,
            string? description,
            TimeSpan? duration,
            DateTime? airingDate,
            Guid? tagId)
        {
            var nextEpisodeNumber = _episodes.Any()
                ? _episodes.Max(e => e.EpisodeNumber) + 1
                : 1;

            var episode = Episode.Create(
                seasonId: Id,
                title: title,
                description: description,
                duration: duration,
                airingDate: airingDate,
                tagId: tagId,
                episodeNumber: nextEpisodeNumber
            );

            _episodes.Add(episode);

            return episode;
        }

        public void AddEpisode(Episode episode)
        {
            if (episode is null)
                throw new ArgumentNullException(nameof(episode));

            if (_episodes.Any(e => e.Id == episode.Id))
                return;

            if (_episodes.Any(e => e.EpisodeNumber == episode.EpisodeNumber))
                throw new InvalidOperationException("Episode number must be unique within a season.");

            _episodes.Add(episode);
        }

        public void RemoveEpisode(Guid episodeId)
        {
            _episodes.RemoveAll(e => e.Id == episodeId);
        }

        public Episode? GetEpisode(Guid episodeId)
        {
            return _episodes.FirstOrDefault(e => e.Id == episodeId);
        }

        public void UpdateDetails(string? title, int? seasonNumber, DateOnly? releaseDate)
        {
            Title = title;

            if (seasonNumber.HasValue)
            {
                if (seasonNumber.Value <= 0)
                    throw new ArgumentException("Season number must be greater than zero.");

                SeasonNumber = seasonNumber.Value;
            }
            ReleaseDate = releaseDate;
        }
    }
}