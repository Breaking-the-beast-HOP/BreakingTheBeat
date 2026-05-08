using Gateway.Contracts.Podcast.Contribution;
using PodcastCatalog.Features.Podcasts.Get;

namespace Gateway.Contracts.Podcast
{
    public class NewPodcastRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public Guid CategoryId { get; set; }
        public Guid LanguageId { get; set; }

        public List<ContributionRequest> Contributions { get; set; } = new();
    }
}
