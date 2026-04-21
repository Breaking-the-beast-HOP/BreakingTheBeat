namespace PodcastCatalog.Shared.Models
{
    public class PodcastCategories
    {
        public Guid PodcastId { get; set; }
        public Podcast Podcast { get; set; } = null!;

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
