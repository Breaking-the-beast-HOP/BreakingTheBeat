using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
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
            var result = await _mediator.Send(new GetContributorRolesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}
