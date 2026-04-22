using Gateway.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Genre;
using MusicCatalog.Features.Track.Create;

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
            var result = await _mediator.Send(new CreateTrackCommand(request.Title, request.Version, request.ISRC, request.GenreId, request.LanguageId), cancellationToken);
            return Ok(result);
      
        }

    }
}
