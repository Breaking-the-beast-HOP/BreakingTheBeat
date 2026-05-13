using Gateway.Contracts.Podcast;
using Gateway.Contracts.Podcast.Contribution;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.Podcasts.Create;
using PodcastCatalog.Features.Podcasts.Delete;
using PodcastCatalog.Features.Podcasts.Get;
using PodcastCatalog.Features.Podcasts.GetAll;
using PodcastCatalog.Features.Podcasts.Shared;
using PodcastCatalog.Features.Podcasts.Update;

namespace Gateway.Controllers.Podcast
{
    [Route("api/podcasts/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        public async Task<IActionResult> CreatePodcast(
            [FromBody] NewPodcastRequest request,
            CancellationToken cancellationToken)
        {
            var contributions = request.Contributions.Select(c => new CreateContributionDTO
            {
                ContributorId = c.ContributorId,
                ContributorRoleId = c.ContributorRoleId,
                HasRights = c.HasRights
            }).ToList();

            var command = new CreatePodcastCommand(
                request.Title,
                request.Description,
                request.CategoryId,
                request.LanguageId,
                contributions
            );

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        // PUT: api/podcasts/podcast/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePodcast(
            Guid id,
            [FromBody] PodcastUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var contributions = request.Contributions.Select(c => new ContributionRowDTO
            {
                Id = c.Id ?? Guid.Empty,
                ContributorId = c.ContributorId,
                ContributorName = c.ContributorName,
                ContributorRoleId = c.ContributorRoleId,
                HasRights = c.HasRights

            }).ToList();
            await _mediator.Send(
                new UpdatePodcastCommand(
                    Id: id,
                    Title: request.Title,
                    Description: request.Description,
                    CategoryId: request.CategoryId,
                    LanguageId: request.LanguageId,
                    contributions),
                cancellationToken);

            return NoContent();
        }


        // GET: api/podcasts/podcast/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PodcastDetailsResponse>> GetPodcast(Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPodcastQuery(id), cancellationToken);

            var response = new PodcastDetailsResponse
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                CategoryId = result.CategoryId,
                CategoryName = result.CategoryName,
                LanguageId = result.LanguageId,
                LanguageName = result.LanguageName,
                Contributions = result.Contributions.Select(c => new ContributionResponse
                {
                    Id = c.Id,
                    ContributorId = c.ContributorId,
                    ContributorName = c.ContributorName,
                    ContributorRoleId = c.ContributorRoleId,
                    ContributorRoleName = c.ContributorRoleName
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet()]
        public async Task<ActionResult<List<PodcastResponse>>> GetAllPodcasts(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllPodcastsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePodcast(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePodcastCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
