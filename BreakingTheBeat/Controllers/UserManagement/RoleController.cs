using Gateway.Contracts.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Features.Roles.GetAll;

namespace Gateway.Controllers.UserManagement;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetRoles")]
    public async Task<ActionResult<List<RoleResponse>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        return Ok(result);
    }
}
