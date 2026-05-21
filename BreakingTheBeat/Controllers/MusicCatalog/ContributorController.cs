using Gateway.Contracts.Music.Contributor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogContext.Features.Contributor.Create;
using MusicCatalogContext.Features.Contributor.Delete;
using MusicCatalogContext.Features.Contributor.Get;
using MusicCatalogContext.Features.Contributor.GetById;

namespace Gateway.Controllers.Music
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ContributorController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ContributorController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Content Creator")]
        public async Task<IActionResult> CreateContributor([FromBody] ContributorRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateContributorCommand(request.LegalName, request.DisplayName, request.IPI, request.ISNI), cancellationToken);
            return Ok(result);

        }

        [HttpGet(Name = "GetContributors")]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<ActionResult<List<ContributorResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetContributorsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<ActionResult<ContributorDetailsResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var contributor = await _mediator.Send(new GetContributorByIdQuery(id), cancellationToken);

            var response = new ContributorDetailsResponse
            {
                Id = contributor.Id,
                DisplayName = contributor.DisplayName,
                LegalName = contributor.LegalName,
                IPI = contributor.IPI,
                ISNI = contributor.ISNI,

                TrackContributions = contributor.TrackContributions
                    .Select(c => new ContributorTrackContributionResponse
                    {
                        TrackId = c.TrackId,
                        TrackTitle = c.TrackTitle,
                        ISRC = c.ISRC,
                        RoleName = c.RoleName
                    })
                    .ToList(),

                ReleaseContributions = contributor.ReleaseContributions
                    .Select(c => new ContributorReleaseContributionResponse
                    {
                        ReleaseId = c.ReleaseId,
                        ReleaseTitle = c.ReleaseTitle,
                        EAN = c.EAN,
                        RoleName = c.RoleName
                    })
                    .ToList()
            };

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Content Creator")]
        public async Task<IActionResult> DeleteContributor(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteContributorCommand(id), cancellationToken);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}

