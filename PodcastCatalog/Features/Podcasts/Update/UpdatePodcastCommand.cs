using MediatR;

namespace PodcastCatalog.Features.Podcasts.Update
{
    public record UpdatePodcastCommand(Guid Id, string Title, string Description, Guid CategoryId, Guid LanguageId) : IRequest<PodcastDTO>
    {
    }
}
