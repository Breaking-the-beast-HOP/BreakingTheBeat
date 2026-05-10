using Gateway.Contracts.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Features.User.GetAll;

namespace Gateway.Controllers.UserManagement
{
    [Route("api/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<List<UserResponse>>> GetUsers(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUsersQuery(), cancellationToken);
            return Ok(result);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser( Guid id, [FromBody] UpdateUserRequest request,CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateUserCommand(
                id,
                request.Email,
                request.Username,
                request.RoleName),
                cancellationToken);

            return NoContent();
        }

        //[HttpPost("{id:guid}/reset-password")]
        //public async Task<IActionResult> ResetPassword( Guid id, ResetPasswordRequest request,CancellationToken cancellationToken)
        //{
        //    await _mediator.Send(new ResetUserPasswordCommand(id,request.NewPassword), cancellationToken);

        //    return NoContent();
        //}

        //[HttpDelete("{id:guid}")]
        //public async Task<IActionResult> DeleteUser(Guid id,CancellationToken cancellationToken)
        //{
        //    await _mediator.Send(new DeleteUserCommand(id), cancellationToken);

        //    return NoContent();
        //}
    }
}