namespace Gateway.Contracts.Podcast
{
    
    public class CreatePodcastSeasonRequest
    {
        public string? Title { get; set; }
        public int SeasonNumber { get; set; }
    }
}
