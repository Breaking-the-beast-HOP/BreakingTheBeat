using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Podcast
{
    [Route("api/podcasts/[controller]")]
    [ApiController]
    public class ContributorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ContributorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetContributors(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PodcastCatalog.Features.Contributors.GetAll.GetAllContributorsQuery(), cancellationToken);
            var responses = result.Select(r => new Contracts.Podcast.Contributor.ContributorResponse
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();
            return Ok(responses);
        }
    }
}
