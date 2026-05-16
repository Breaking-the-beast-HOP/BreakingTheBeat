using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MusicCatalogContext.Features.Genre;
using MusicCatalogContext.Features.Genre.Interfaces;

namespace MusicCatalogContext.Features.Track.Get
{
    public class GetTracksHandler : IRequestHandler<GetTracksQuery, List<TrackDTO>>
    {
        private readonly ITrackRepository _repository;

        public GetTracksHandler(ITrackRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TrackDTO>> Handle(GetTracksQuery request, CancellationToken cancellationToken)
        {


            var tracks = await _repository.GetAllAsync();

            return tracks.Select(t => new TrackDTO
            {
                Id = t.Id,
                Title = t.Title,
                Version = t.Version,
                ISRC = t.ISRC,
                GenreId = t.GenreId,
                GenreName = t.Genre.Name,
                MetadataLanguageId = t.MetadataLanguageId,
                MetadataLanguageName = t.MetadataLanguage.Name,
                AudioLanguageId = t.AudioLanguageId,
                AudioLanguageName = t.AudioLanguage.Name
            }).ToList();
        }
    }

}
