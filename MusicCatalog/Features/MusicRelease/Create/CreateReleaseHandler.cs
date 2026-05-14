using MediatR;

namespace MusicCatalog.Features.Release.Create
{
    public class CreateReleaseHandler : IRequestHandler<CreateReleaseCommand, Guid>
    {
        private readonly IReleaseRepository _repository;

        public CreateReleaseHandler(IReleaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateReleaseCommand request, CancellationToken cancellationToken)
        {
            var release = Domain.Entities.Release.Create(
                request.Title,
                request.Version,
                request.EAN,
                request.ReleaseDate,
                request.GenreId,
                request.LanguageId,
                request.Status
            );

            foreach (var volumeRequest in request.Volumes.OrderBy(v => v.VolumeNumber))
            {
                var volume = release.AddVolume(volumeRequest.VolumeNumber);

                foreach (var trackRequest in volumeRequest.Tracks.OrderBy(t => t.TrackNumber))
                {
                    volume.AddTrack(
                        trackRequest.TrackId,
                        trackRequest.TrackNumber
                    );
                }
            }

            foreach (var contribution in request.Contributions)
            {
                foreach (var roleId in contribution.RoleIds)
                {
                    release.AddContribution(contribution.ContributorId, roleId);
                }
            }

            await _repository.AddAsync(release);

            return release.Id;
        }
    }
}