namespace Gateway.Contracts.Podcast
{
    public class CreatePodcastEpisodeRequest
    {
        public string Title { get; set; } = "";
        public string? Description { get; set; }

        public int EpisodeNumber { get; set; }

        public TimeSpan? Duration { get; set; }

        public DateOnly AiringDate { get; set; }

        public Guid? TagId { get; set; }
    }
}
