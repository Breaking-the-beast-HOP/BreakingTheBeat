using MediatR;

namespace MusicCatalog.Features.Contributor.Delete
{
    public record DeleteContributorCommand(Guid Id) : IRequest<bool>;
}