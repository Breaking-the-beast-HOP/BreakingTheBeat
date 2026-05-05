using MediatR;

namespace PodcastCatalog.Features.Contributions.AddToPodcast
{
    public record AddPodcastContributionCommand(Guid PodcastId, Guid ContributorId, Guid ContributorRoleId, bool HasRights) : IRequest<ContributionDTO>;
}
