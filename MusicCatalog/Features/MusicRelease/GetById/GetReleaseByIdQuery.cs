using MediatR;

namespace MusicCatalogContext.Features.Release.GetById
{
    public record GetReleaseByIdQuery(Guid Id) : IRequest<ReleaseDetailsDto?>;
}