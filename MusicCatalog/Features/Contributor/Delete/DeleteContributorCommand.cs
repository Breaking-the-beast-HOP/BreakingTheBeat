using MediatR;

namespace MusicCatalogContext.Features.Contributor.Delete
{
    public record DeleteContributorCommand(Guid Id) : IRequest<bool>;
}