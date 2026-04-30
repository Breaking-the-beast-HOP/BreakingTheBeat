using Gateway.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Track.Create;
using MusicCatalog.Features.Track.Delete;
using MusicCatalog.Features.Track.Get;
using MusicCatalog.Features.Track.GetById;
using MusicCatalog.Features.Track.Update;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
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
        public async Task<ActionResult<List<TrackResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTracksQuery(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
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

        .Select(c => new Contracts.ContributionResponse
        {
            ContributorId = c.ContributorId,
            ContributorName = c.ContributorName,
            Roles = c.Roles.Select(r => new Contracts.ContributionRoleResponse
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
        public async Task<IActionResult> UpdateTrack( Guid id,[FromBody] UpdateTrackRequest request,CancellationToken cancellationToken)
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
        public async Task<IActionResult> DeleteTrack(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteTrackCommand(id), cancellationToken);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
