using MediatR;

namespace MusicCatalogContext.Features.Contributor.Create
{
    public record CreateContributorCommand(string DisplayName, string LegalName, string IPI, string ISNI) : IRequest<Guid>;
}
