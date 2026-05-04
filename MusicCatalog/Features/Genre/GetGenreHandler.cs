using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MusicCatalog.Features.Genre.Interfaces;
using MusicCatalog.Features.Language.GetLanguages;
using MusicCatalog.Features.Language.Interfaces;

namespace MusicCatalog.Features.Genre
{
    public class GetGenreHandler : IRequestHandler<GetGenresQuery, List<GenreDTO>>
    {
        private readonly IGenreRepository _repository;

        public GetGenreHandler(IGenreRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GenreDTO>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var Genres = await _repository.GetAllAsync();

            return Genres.Select(l => new GenreDTO
            {
                Id = l.Id,
                Name = l.Name,
            }).ToList();
        }
    }
}
