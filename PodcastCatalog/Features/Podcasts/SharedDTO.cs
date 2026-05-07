namespace PodcastCatalog.Features.Podcasts
{
    public class SharedDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        // Contributor details
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }

        // Language details
        public Guid LanguageId { get; set; }
        public string? LanguageName { get; set; }

        // List of contributors
        public List<ContributionRowDTO> Contributions { get; set; }
    }
}
