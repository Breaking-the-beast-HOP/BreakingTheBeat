using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Update
{
    public record UpdatePodcastCommand(Guid Id, string Title, string Description, Guid CategoryId, Guid LanguageId, List<ContributionRowDTO> Contributors) : IRequest<PodcastDTO>
    {
    }
}
