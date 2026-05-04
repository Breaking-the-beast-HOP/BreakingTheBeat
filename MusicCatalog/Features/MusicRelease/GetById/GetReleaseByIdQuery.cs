using MediatR;

namespace MusicCatalog.Features.Release.GetById
{
    public record GetReleaseByIdQuery(Guid Id) : IRequest<ReleaseDetailsDto?>;
}