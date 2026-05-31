using Gateway.Contracts.UserManagement;
using Gateway.Controllers.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using UserManagement.Features.User.Login;

namespace BreakingTheBeat.Tests.Gateway;

public class AuthControllerTests
{
    private readonly IMediator _mediator;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new AuthController(_mediator);
    }

    [Fact]
    public async Task Login_DispatchesCommandToMediator()
    {
        _mediator.Send(Arg.Any<LoginUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(new LoginUserResult { Succeeded = true, Token = "tok" });

        await _controller.Login(
            new LoginUserRequest { Email = "user@example.com", Password = "pw" },
            CancellationToken.None);

        await _mediator.Received(1).Send(
            Arg.Is<LoginUserCommand>(c => c.Email == "user@example.com" && c.Password == "pw"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Login_OnSuccess_ReturnsOkWithResult()
    {
        var expected = new LoginUserResult { Succeeded = true, Token = "tok" };
        _mediator.Send(Arg.Any<LoginUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var actionResult = await _controller.Login(
            new LoginUserRequest { Email = "user@example.com", Password = "pw" },
            CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Same(expected, ok.Value);
    }

    [Fact]
    public async Task Login_OnFailure_ReturnsBadRequestWithErrors()
    {
        var failure = new LoginUserResult
        {
            Succeeded = false,
            Errors = new List<string> { "Forkert email eller adgangskode." }
        };
        _mediator.Send(Arg.Any<LoginUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(failure);

        var actionResult = await _controller.Login(
            new LoginUserRequest { Email = "user@example.com", Password = "wrong" },
            CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult);
        Assert.Same(failure.Errors, badRequest.Value);
    }
}
