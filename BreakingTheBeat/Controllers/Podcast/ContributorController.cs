using Gateway.Contracts.Podcast.Contributor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.Contributors.Create;

namespace Gateway.Controllers.Podcast
{
    [Route("api/podcasts/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ContributorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ContributorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
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

        [HttpPost]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<IActionResult> CreateContributor([FromBody] CreatePodcastContributorRequest request,CancellationToken cancellationToken)
        {
            var contributorId = await _mediator.Send(
                new CreatePodcastContributorCommand(
                    request.DisplayName,
                    request.FullName,
                    request.Title,
                    request.Biography,
                    request.Expertise,
                    request.ProfileUrl),
                cancellationToken);

            return Ok(contributorId);
        }
    }
}
