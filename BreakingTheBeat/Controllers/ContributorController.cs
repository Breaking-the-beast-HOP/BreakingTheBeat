using Gateway.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Contributor.Create;
using MusicCatalog.Features.Contributor.Get;
using MusicCatalog.Features.Genre;
using MusicCatalog.Features.Track.Create;

namespace Gateway.Controllers
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
    }
}
