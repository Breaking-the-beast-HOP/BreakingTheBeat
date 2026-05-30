using MediatR;

namespace PodcastCatalog.Features.Contributors.Create
{

    public record CreatePodcastContributorCommand(
        string DisplayName,
        string? FullName,
        string? Title,
        string? Biography,
        string? Expertise,
        string? ProfileUrl
    ) : IRequest<Guid>;
}
