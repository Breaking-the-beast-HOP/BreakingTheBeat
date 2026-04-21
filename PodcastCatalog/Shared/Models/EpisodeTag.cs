namespace PodcastCatalog.Shared.Models
{
    public class EpisodeTag
    {
        public Guid EpisodeId { get; set; }
        public Episode Episode { get; set; } = null!;
        public Guid TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
