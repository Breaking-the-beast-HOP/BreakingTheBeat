namespace PodcastCatalog.Features.Podcasts.Shared
{
    public class PodcastDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public Guid LanguageId { get; set; }
        public string? LanguageName { get; set; }

        public List<ContributionRowDTO> Contributions { get; set; } = new();

        public List<SeasonDTO> Seasons { get; set; } = new();
    }

    public class SeasonDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int SeasonNumber { get; set; }
        public DateOnly? ReleaseDate { get; set; }

        public List<EpisodeDTO> Episodes { get; set; } = new();
    }

    public class EpisodeDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? AiringDate { get; set; }
        public Guid? TagId { get; set; }
        public string? TagName { get; set; }
        public int EpisodeNumber { get; set; }
        public string Status { get; set; } = "Draft";
    }
}
