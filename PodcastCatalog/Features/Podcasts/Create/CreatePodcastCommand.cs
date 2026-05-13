using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Create
{
    public record CreatePodcastCommand(string Title, string Description, Guid CategoryId, Guid LanguageId, List<CreateContributionDTO> Contributors) : IRequest<PodcastDTO>;

}
