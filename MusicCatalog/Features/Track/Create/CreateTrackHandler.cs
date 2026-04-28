using MediatR;

namespace MusicCatalog.Features.Track.Create
{
    public class CreateTrackHandler : IRequestHandler<CreateTrackCommand, Guid>
    {
        private readonly ITrackRepository _repository;

        public CreateTrackHandler(ITrackRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
        {

            var track = Domain.Entities.Track.Create(
                  request.Title,
                  request.Version,
                  request.ISRC,
                  request.GenreId,
                  request.AudioLanguageId,
                  request.MetadataLanguageId);

            foreach (var contribution in request.Contributions)
            {
                foreach (var roleId in contribution.RoleIds)
                {
                    track.AddContribution(contribution.ContributorId, roleId);
                }
            }

            await _repository.AddAsync(track);

            return track.Id;
        }
    }
}
