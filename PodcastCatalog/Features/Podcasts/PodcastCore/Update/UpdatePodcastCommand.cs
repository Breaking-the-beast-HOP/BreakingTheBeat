using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.Update;

public record UpdatePodcastCommand(Guid Id, string? Title, string? Description, Guid? CategoryId, Guid? LanguageId, List<UpdatePodcastContributionInput> Contributions) : IRequest<PodcastDTO>;

public record UpdatePodcastContributionInput(Guid ContributorId, Guid ContributorRoleId, bool HasRights = false);