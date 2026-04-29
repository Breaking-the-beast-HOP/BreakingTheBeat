using MediatR;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Features.Track.Update
{
    public class UpdateTrackHandler : IRequestHandler<UpdateTrackCommand, bool>
    {
        private readonly ITrackRepository _repository;

        public UpdateTrackHandler(ITrackRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetByIdAsync(request.Id);

            if (track is null)
                return false;

            track.UpdateDetails(
                request.Title,
                request.Version,
                request.ISRC,
                request.GenreId,
                request.MetadataLanguageId,
                request.AudioLanguageId
            );

            await _repository.UpdateAsync(track);

            return true;
        }
    }
}
