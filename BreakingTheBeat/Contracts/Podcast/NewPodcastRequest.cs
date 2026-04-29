using PodcastCatalog.Domain.Entities;

namespace Gateway.Contracts.Podcast
{
    public class NewPodcastRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid LanguageId { get; set; }
    }
}
