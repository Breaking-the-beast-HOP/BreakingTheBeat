namespace PodcastCatalog.Features.Contributors.GetAll
{
    public record ContributorDTO(
       Guid Id,
       string DisplayName,
       string? FullName,
       string? Title,
       string? Biography,
       string? Expertise
   );
}