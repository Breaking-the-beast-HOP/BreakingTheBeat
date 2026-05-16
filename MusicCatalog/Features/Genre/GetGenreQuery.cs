using MediatR;

namespace MusicCatalogContext.Features.Genre
{
    public record GetGenresQuery : IRequest<List<GenreDTO>>;

}
