namespace Gateway.Contracts.Podcast.Contributor
{
    public record CreatePodcastContributorRequest(
     string DisplayName,
     string? FullName,
     string? Title,
     string? Biography,
     string? Expertise,
     string? ProfileUrl
 );
}
