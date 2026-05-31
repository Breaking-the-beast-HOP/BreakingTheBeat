using MediatR;

namespace MusicCatalogContext.Features.MusicRelease.GetAll
{
    public record GetAllReleasesQuery : IRequest<List<ReleaseDto>>;
}