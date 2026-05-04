using MediatR;

namespace MusicCatalog.Features.Release.Update
{
    public class UpdateReleaseHandler : IRequestHandler<UpdateReleaseCommand, bool>
    {
        private readonly IReleaseRepository _repository;

        public UpdateReleaseHandler(IReleaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateReleaseCommand request, CancellationToken cancellationToken)
        {
            var release = await _repository.GetByIdAsync(request.Id);

            if (release is null)
                return false;

            release.UpdateDetails(
                request.Title,
                request.Version,
                request.EAN,
                request.GenreId,
                request.MetadataLanguageId

            );

            release.UpdateContributions(
                request.Contributions
                    .Select(c => (c.ContributorId, c.ContributorRoleId))
                    .ToList()
            );

            release.UpdateVolumes(
                request.Volumes
                    .Select(v => (
                        VolumeNumber: v.VolumeNumber,
                        Tracks: v.Tracks
                            .Select(t => (
                                TrackId: t.TrackId,
                                TrackNumber: t.TrackNumber
                            ))
                            .ToList()
                    ))
                    .ToList()
            );

            await _repository.UpdateAsync(release);

            return true;
        }
    }
}
