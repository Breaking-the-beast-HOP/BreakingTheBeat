using Gateway.Contracts.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Features.User.Login;
using UserManagement.Features.User.Register;

namespace Gateway.Controllers.UserManagement;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new List<string> { "Email er påkrævet." });

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new List<string> { "Adgangskode er påkrævet." });

        var result = await _mediator.Send(
            new RegisterUserCommand(request.Email, request.Username, request.Password, request.RoleName),
            cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result.UserId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new LoginUserCommand(request.Email, request.Password),
            cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result);
    }
}
