using MediatR;

namespace MusicCatalog.Features.Release.GetAll
{
    public record GetAllReleasesQuery : IRequest<List<ReleaseDto>>;
}