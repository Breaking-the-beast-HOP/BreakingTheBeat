using Gateway.Contracts.Podcast.Contribution;

namespace Gateway.Contracts.Podcast
{

    public class PodcastDetailsResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public Guid LanguageId { get; set; }
        public string? LanguageName { get; set; }

        public List<ContributionResponse> Contributions { get; set; } = new();

        public List<SeasonResponse> Seasons { get; set; } = new();
    }

    public class SeasonResponse
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public int SeasonNumber { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public List<EpisodeResponse> Episodes { get; set; } = new();
    }

    public class EpisodeResponse
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int EpisodeNumber { get; set; }

        public string Status { get; set; } = "Draft";

        public List<ContributionResponse> Contributions { get; set; } = new();
    }

}