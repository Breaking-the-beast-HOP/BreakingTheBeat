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
    }
}