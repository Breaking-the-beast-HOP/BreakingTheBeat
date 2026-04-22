using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.Podcasts.Create;
using PodcastCatalog.Features.Podcasts.Get;
using PodcastCatalog.Features.Podcasts.Update;

namespace Gateway.Controllers.Podcast
{
    [Route("api/podcasts/[controller]")]
    [ApiController]
    public class PodcastController : ControllerBase
    {
        public readonly IMediator _mediator;

        public PodcastController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST: api/podcasts/podcast
        [HttpPost]
        public async Task<ActionResult<PodcastDTO>> CreatePodcast(
            [FromBody] CreatePodcastCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetPodcast), new { id = result.Id }, result);
        }

        // PUT: api/podcasts/podcast/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PodcastDTO>> UpdatePodcast(
            Guid id,
            [FromBody] UpdatePodcastCommand command,
            CancellationToken cancellationToken)
        {
            var updatedCommand = command with { Id = id };
            var result = await _mediator.Send(updatedCommand, cancellationToken);
            return Ok(result);
        }

        // GET: api/podcasts/podcast/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PodcastDTO>> GetPodcast(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPodcastQuery(id), cancellationToken);
            return Ok(result);
        }

    }
}
