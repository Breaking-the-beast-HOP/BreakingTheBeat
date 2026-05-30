using Gateway.Contracts.Podcast;
using Gateway.Contracts.Podcast.Contribution;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.Podcasts.Episodes.AddToPodcast;
using PodcastCatalog.Features.Podcasts.Episodes.RemoveFromPodcast;
using PodcastCatalog.Features.Podcasts.PodcastCore.Create;
using PodcastCatalog.Features.Podcasts.PodcastCore.Delete;
using PodcastCatalog.Features.Podcasts.PodcastCore.Get;
using PodcastCatalog.Features.Podcasts.PodcastCore.GetAll;
using PodcastCatalog.Features.Podcasts.PodcastCore.Update;
using PodcastCatalog.Features.Podcasts.Seasons.AddToPodcast;
using PodcastCatalog.Features.Podcasts.Seasons.RemoveFromPodcast;
using PodcastCatalog.Features.Podcasts.Shared;

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
        public async Task<IActionResult> CreatePodcast([FromBody] NewPodcastRequest request, CancellationToken cancellationToken)
        {
            var contributions = request.Contributions?
                .Select(c => new CreateContributionDTO
                {
                    ContributorId = c.ContributorId,
                    ContributorRoleId = c.ContributorRoleId
                })
                .ToList() ?? new List<CreateContributionDTO>();

            var seasons = request.Seasons?
                 .Select(s => new CreateSeasonDTO
                 {
                     Title = s.Title,
                     SeasonNumber = s.SeasonNumber,
                     Episodes = s.Episodes.Select(e => new CreateEpisodeDTO
                     {
                         Title = e.Title,
                         Description = e.Description,
                         Duration = e.Duration,
                         AiringDate = e.AiringDate,
                         TagId = e.TagId,
                         Status = e.Status,
                         Contributions = e.Contributions?
                            .Select(c => new CreateContributionDTO
                            {
                                ContributorId = c.ContributorId,
                                ContributorRoleId = c.ContributorRoleId
                            })
                            .ToList() ?? new List<CreateContributionDTO>()
                     }).ToList()
                 })
                 .ToList() ?? new List<CreateSeasonDTO>();

            var command = new CreatePodcastCommand(
                request.Title,
                request.Description,
                request.CategoryId,
                request.LanguageId,
                contributions,
                seasons
            );

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        // PUT: api/podcasts/podcast/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePodcast(Guid id, [FromBody] PodcastUpdateRequest request, CancellationToken cancellationToken)
        {
            var contributions = request.Contributions.Select(c =>
                new UpdatePodcastContributionInput(
                    c.ContributorId,
                    c.ContributorRoleId
                   
                )).ToList();

            await _mediator.Send(
                new UpdatePodcastCommand(
                    Id: id,
                    Title: request.Title,
                    Description: request.Description,
                    CategoryId: request.CategoryId,
                    LanguageId: request.LanguageId,
                    Contributions: contributions
                ),
                cancellationToken);

            return NoContent();
        }


        // GET: api/podcasts/podcast/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PodcastDetailsResponse>> GetPodcast(Guid id, CancellationToken cancellationToken)
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
                    ContributorRoleName = c.ContributorRoleName,
                    //HasRights = c.HasRights
                }).ToList(),

                Seasons = result.Seasons.Select(s => new SeasonResponse
                {
                    Id = s.Id,
                    Title = s.Title,
                    SeasonNumber = s.SeasonNumber,
                    ReleaseDate = s.ReleaseDate,

                    Episodes = s.Episodes.Select(e => new EpisodeResponse
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Description = e.Description,
                        EpisodeNumber = e.EpisodeNumber,
                        Status = e.Status,
                        Contributions = e.Contributions.Select(c => new ContributionResponse
                        {
                            Id = c.Id,
                            ContributorId = c.ContributorId,
                            ContributorName = c.ContributorName,
                            ContributorRoleId = c.ContributorRoleId,
                            ContributorRoleName = c.ContributorRoleName
                        }).ToList()
                    }).ToList()
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

        [HttpPost("{podcastId:guid}/seasons")]
        public async Task<IActionResult> CreateSeason(Guid podcastId, [FromBody] CreatePodcastSeasonRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new AddSeasonCommand(
                    PodcastId: podcastId,
                    Title: request.Title,
                    SeasonNumber: request.SeasonNumber
                ),
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("seasons/{seasonId:guid}")]
        public async Task<IActionResult> DeleteSeason(Guid seasonId, CancellationToken cancellationToken)
        {
            var deleted = await _mediator.Send(
                new DeletePodcastSeasonCommand(seasonId),
                cancellationToken);

            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("seasons/{seasonId:guid}/episodes")]
        public async Task<IActionResult> CreateEpisode(Guid seasonId, [FromBody] CreatePodcastEpisodeRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new CreatePodcastEpisodeCommand(
                    SeasonId: seasonId,
                    Title: request.Title,
                    Description: request.Description,
                    EpisodeNumber: request.EpisodeNumber,
                    Duration: request.Duration,
                    AiringDate: request.AiringDate,
                    TagId: request.TagId
                ),
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("episodes/{episodeId:guid}")]
        public async Task<IActionResult> DeleteEpisode(
            Guid episodeId,
            CancellationToken cancellationToken)
        {
            var deleted = await _mediator.Send(
                new DeletePodcastEpisodeCommand(episodeId),
                cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
    }
}
