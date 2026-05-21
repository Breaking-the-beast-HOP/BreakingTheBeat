using MediatR;
using MusicCatalogContext.Domain.Entities;

namespace MusicCatalogContext.Features.Track.Update
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

            track.UpdateContributions(
                request.Contributions
                    .Select(c => (c.ContributorId, c.ContributorRoleId))
                    .ToList()
            );

            await _repository.UpdateAsync(track);

            return true;
        }
    }
}
