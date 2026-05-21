using Gateway.Contracts.Music.Contribution;
using Gateway.Contracts.Music.ContributionRole;
using Gateway.Contracts.Music.Track;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogContext.Features.Track.Create;
using MusicCatalogContext.Features.Track.Delete;
using MusicCatalogContext.Features.Track.Get;
using MusicCatalogContext.Features.Track.GetById;
using MusicCatalogContext.Features.Track.Update;

namespace Gateway.Controllers.Music
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Content Creator")]
        public async Task<IActionResult> CreateTrack([FromBody] TrackRequest request, CancellationToken cancellationToken)
        {

            var contributions = request.Contributions
                .Select(c => new CreateContributionCommand(
                    c.ContributorId,
                    c.RoleIds))
                .ToList();

            var command = new CreateTrackCommand(
                request.Title,
                request.Version,
                request.ISRC,
                request.GenreId,
                request.AudioLanguageId,
                request.MetadataLanguageId,
                contributions);

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);

        }

        [HttpGet(Name = "GetTracks")]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<ActionResult<List<TrackResponse>>> Get(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Authenticated: {User.Identity?.IsAuthenticated}");
            var result = await _mediator.Send(new GetTracksQuery(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<ActionResult<TrackDetailsResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var track = await _mediator.Send(new GetTrackByIdQuery(id), cancellationToken);
            var response = new TrackDetailsResponse
            {
                Id = track.Id,
                Title = track.Title,
                Version = track.Version,
                ISRC = track.ISRC,
                DurationSeconds = (int)track.Duration.TotalSeconds,

                GenreName = track.GenreName,
                GenreId = track.GenreId,
                MetadataLanguageName = track.MetadataLanguageName,
                MetadataLanguageId = track.MetadataLanguageId,
                AudioLanguageName = track.AudioLanguageName,
                AudioLanguageId = track.AudioLanguageId,

                Contributions = track.Contributions

        .Select(c => new ContributionResponse
        {
            ContributorId = c.ContributorId,
            ContributorName = c.ContributorName,
            Roles = c.Roles.Select(r => new ContributionRoleResponse
            {
                Id = r.Id,
                Name = r.Name
            }).ToList()
        })
        .ToList()
            };

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Content Creator")]
        public async Task<IActionResult> UpdateTrack(Guid id, [FromBody] UpdateTrackRequest request, CancellationToken cancellationToken)
        {
            var contributions = request.Contributions
                .SelectMany(c => c.RoleIds.Select(roleId =>
                    new UpdateContributionCommand(
                        c.ContributorId,
                        roleId)))
                .ToList();

            var result = await _mediator.Send(
                new UpdateTrackCommand(
                    id,
                    request.Title,
                    request.Version,
                    request.ISRC,
                    request.GenreId,
                    request.MetadataLanguageId,
                    request.AudioLanguageId,
                    contributions),
                cancellationToken);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Content Creator")]
        public async Task<IActionResult> DeleteTrack(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteTrackCommand(id), cancellationToken);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
