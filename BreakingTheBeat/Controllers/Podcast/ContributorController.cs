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
            var result = await _mediator.Send(
                new PodcastCatalog.Features.Contributors.GetAll.GetAllContributorsQuery(),
                cancellationToken);

            var response = result.Select(c =>
                new PodcastContributorListItemResponse(
                    c.Id,
                    c.DisplayName,
                    c.FullName,
                    c.Title,
                    c.Biography,
                    c.Expertise
                )).ToList();

            return Ok(response);
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
