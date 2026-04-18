using MediatR;

namespace MusicCatalog.Features.Genre
{
    public record GetGenresQuery : IRequest<List<GenreDTO>>;
    
}
