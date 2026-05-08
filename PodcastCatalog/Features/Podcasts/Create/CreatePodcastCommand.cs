using MediatR;

namespace PodcastCatalog.Features.Podcasts.Create
{
    public record CreatePodcastCommand(string Title, string Description, Guid CategoryId, Guid LanguageId, List<CreateContributionDTO> Contributors) : IRequest<PodcastDTO>;

}
