using Gateway.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Genre;
using MusicCatalog.Features.Language.GetLanguages;
using MusicCatalog.Features.Track.Create;
using MusicCatalog.Features.Track.Get;
using MusicCatalog.Features.Track.GetById;

namespace Gateway.Controllers.Music
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
            var result = await _mediator.Send(new CreateTrackCommand(request.Title, request.Version, request.ISRC, request.GenreId, request.AudioLanguageId, request.MetadataLanguageId), cancellationToken);
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
            var result = await _mediator.Send(new GetTrackByIdQuery(id), cancellationToken);

            return Ok(result);
        }
    }
}
