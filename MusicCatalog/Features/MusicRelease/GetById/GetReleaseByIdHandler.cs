using MediatR;

namespace MusicCatalogContext.Features.MusicRelease.GetById
{
    public class GetReleaseByIdHandler : IRequestHandler<GetReleaseByIdQuery, ReleaseDetailsDto?>
    {
        private readonly IReleaseRepository _repository;

        public GetReleaseByIdHandler(IReleaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReleaseDetailsDto?> Handle(GetReleaseByIdQuery request, CancellationToken cancellationToken)
        {
            var release = await _repository.GetByIdAsync(request.Id);

            if (release is null)
                return null;

            return new ReleaseDetailsDto
            {
                Id = release.Id,
                Title = release.Title,
                Version = release.Version,
                EAN = release.EAN,
                ReleaseDate = release.ReleaseDate,

                GenreId = release.GenreId,
                GenreName = release.Genre?.Name,

                LanguageId = release.LanguageId,
                LanguageName = release.Language?.Name,

                HasCover = false,
                HasBooklet = false,
                Status = release.Status,

                Volumes = release.Volumes
                    .OrderBy(v => v.VolumeNumber)
                    .Select(v => new ReleaseVolumeDto
                    {
                        Id = v.Id,
                        VolumeNumber = v.VolumeNumber,
                        Tracks = v.Tracks
                            .OrderBy(t => t.TrackNumber)
                            .Select(t => new ReleaseTrackDto
                            {
                                TrackId = t.TrackId,
                                TrackNumber = t.TrackNumber,
                                Title = t.Track.Title,
                                Version = t.Track.Version,
                                ISRC = t.Track.ISRC,
                                DurationSeconds = (int)t.Track.Duration.TotalSeconds
                            })
                            .ToList()
                    })
                    .ToList(),

                Contributions = release.Contributions
                    .GroupBy(c => new
                    {
                        c.ContributorId,
                        c.Contributor.DisplayName
                    })
                    .Select(g => new ContributionDto
                    {
                        ContributorId = g.Key.ContributorId,
                        ContributorName = g.Key.DisplayName,
                        Roles = g
                            .Select(c => new ContributionRoleDto
                            {
                                Id = c.ContributorRoleId,
                                Name = c.Role.Name
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}