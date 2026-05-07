namespace Gateway.Contracts.Podcast.Contributor
{
    public class ContributorResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
