using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.ContributorRole;

namespace Gateway.Controllers.Music
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ContributorRoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContributorRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetContributorRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new MusicCatalog.Features.ContributorRole.GetAll.GetContributorRolesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}
