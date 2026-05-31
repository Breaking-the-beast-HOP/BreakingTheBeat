using MediatR;

namespace MusicCatalogContext.Features.MusicRelease.GetById
{
    public record GetReleaseByIdQuery(Guid Id) : IRequest<ReleaseDetailsDto?>;
}