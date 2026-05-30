namespace Gateway.Contracts.Podcast.Contributor
{
    public record PodcastContributorListItemResponse(
     Guid Id,
     string DisplayName,
     string? FullName,
     string? Title,
     string? Biography,
     string? Expertise
 );
}
