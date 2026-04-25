using MediatR;
using MusicCatalog.Features.Genre.Interfaces;
using MusicCatalog.Features.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Features.Track.GetById
{
    public class GetTrackByIdHandler : IRequestHandler<GetTrackByIdQuery, TrackDTO>
    {
        private readonly ITrackRepository _repository;

        public GetTrackByIdHandler(ITrackRepository repository)
        {
            _repository = repository;
        }

        public async Task<TrackDTO> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Track track = await _repository.GetByIdAsync(request.Id);

            TrackDTO trackDTO = new TrackDTO
            {
                Id = track.Id,
                Title = track.Title,
                Version = track.Version,
                ISRC = track.ISRC,
                GenreId = track.GenreId,
                GenreName = track.Genre.Name,
                MetadataLanguageId = track.MetadataLanguageId,
                MetadataLanguageName = track.MetadataLanguage.Name,
                AudioLanguageId = track.AudioLanguageId,
                AudioLanguageName = track.AudioLanguage.Name
            };
            return trackDTO;
        }
    }

}
