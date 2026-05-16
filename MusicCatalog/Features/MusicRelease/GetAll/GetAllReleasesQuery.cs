using MediatR;

namespace MusicCatalogContext.Features.Release.GetAll
{
    public record GetAllReleasesQuery : IRequest<List<ReleaseDto>>;
}