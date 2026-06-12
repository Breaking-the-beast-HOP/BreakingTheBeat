using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogContext.Features.ContributorRole.GetAll;

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
        [Authorize(Roles = "Admin,Content creator,Quality controller")]
        public async Task<IActionResult> GetContributorRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetContributorRolesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}
