using Gateway.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Release;
using MusicCatalog.Features.Release.Create;

namespace Gateway.Controllers
{
  
        [Route("api/[controller]")]
        [ApiController]
        public class ReleaseController : ControllerBase
        {
            private readonly IMediator _mediator;

            public ReleaseController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpPost]
            public async Task<IActionResult> CreateRelease([FromBody] CreateReleaseRequest request,CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                    return BadRequest("Title is required.");

                if (request.GenreId is null || request.GenreId == Guid.Empty)
                    return BadRequest("Genre is required.");

                if (request.LanguageId is null || request.LanguageId == Guid.Empty)
                    return BadRequest("Language is required.");

                if (request.Volumes is null || !request.Volumes.Any())
                    return BadRequest("At least one volume is required.");

                if (request.Volumes.Any(v => v.Tracks is null || !v.Tracks.Any()))
                    return BadRequest("Each volume must contain at least one track.");

                if (request.Volumes
                    .SelectMany(v => v.Tracks)
                    .Any(t => t.TrackId == Guid.Empty))
                {
                    return BadRequest("All track rows must have a selected track.");
                }

                var command = new CreateReleaseCommand(
                    request.Title,
                    request.Version,
                    request.EAN,
                    request.ReleaseDate,
                    request.GenreId.Value,
                    request.LanguageId.Value,
                    request.Volumes.Select(v => new CreateReleaseVolumeCommand(
                        v.VolumeNumber,
                        v.Tracks.Select(t => new CreateReleaseTrackCommand(
                            t.TrackNumber,
                            t.TrackId
                        )).ToList()
                    )).ToList(),
                    request.Contributions.Select(c => new CreateReleaseContributionCommand(
                        c.ContributorId,
                        c.RoleIds
                    )).ToList()
                );

                var result = await _mediator.Send(command, cancellationToken);

                return Ok(result);
        }

        }
    }
