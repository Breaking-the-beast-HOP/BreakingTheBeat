using Gateway.Contracts.Music.Contributor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Contributor.Create;
using MusicCatalog.Features.Contributor.Get;
using MusicCatalog.Features.Contributor.GetById;

namespace Gateway.Controllers.Music
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributorController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ContributorController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateContributor([FromBody] ContributorRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateContributorCommand(request.LegalName, request.DisplayName, request.IPI, request.ISNI), cancellationToken);
            return Ok(result);

        }

        [HttpGet(Name = "GetContributors")]
        public async Task<ActionResult<List<ContributorResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetContributorsQuery(), cancellationToken);
            return Ok(result);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ContributorDetailsResponse>> Get( Guid id,CancellationToken cancellationToken)
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
    }
}

