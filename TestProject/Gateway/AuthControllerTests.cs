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
        // Arrange fælles setup for alle tests.
        // Vi mocker IMediator, så controlleren kan testes uden at kalde den rigtige login-handler.
        _mediator = Substitute.For<IMediator>();
        _controller = new AuthController(_mediator);
    }

    // [Fact]
    // Bruges fordi vi tester ét konkret scenarie:
    // At controlleren oversætter requesten til en LoginUserCommand og sender den via MediatR.
    [Fact]
    public async Task Login_DispatchesCommandToMediator()
    {
        // Arrange
        _mediator.Send(Arg.Any<LoginUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(new LoginUserResult { Succeeded = true, Token = "tok" });

        var request = new LoginUserRequest
        {
            Email = "user@example.com",
            Password = "pw"
        };

        // Act
        await _controller.Login(request, CancellationToken.None);

        // Assert
        await _mediator.Received(1).Send(
            Arg.Is<LoginUserCommand>(command =>
                command.Email == "user@example.com" &&
                command.Password == "pw"),
            Arg.Any<CancellationToken>());
    }

    // [Fact]
    // Bruges fordi vi tester ét konkret succes-scenarie:
    // Når login lykkes, skal controlleren returnere HTTP 200 OK med resultatet.
    [Fact]
    public async Task Login_OnSuccess_ReturnsOkWithResult()
    {
        // Arrange
        var expected = new LoginUserResult
        {
            Succeeded = true,
            Token = "tok"
        };

        _mediator.Send(Arg.Any<LoginUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var request = new LoginUserRequest
        {
            Email = "user@example.com",
            Password = "pw"
        };

        // Act
        var actionResult = await _controller.Login(request, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Same(expected, ok.Value);
    }

    // [Fact]
    // Bruges fordi vi tester ét konkret fejl-scenarie:
    // Når login fejler, skal controlleren returnere HTTP 400 BadRequest med fejlbeskederne.
    [Fact]
    public async Task Login_OnFailure_ReturnsBadRequestWithErrors()
    {
        // Arrange
        var failure = new LoginUserResult
        {
            Succeeded = false,
            Errors = new List<string>
            {
                "Forkert email eller adgangskode."
            }
        };

        _mediator.Send(Arg.Any<LoginUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(failure);

        var request = new LoginUserRequest
        {
            Email = "user@example.com",
            Password = "wrong"
        };

        // Act
        var actionResult = await _controller.Login(request, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult);
        Assert.Same(failure.Errors, badRequest.Value);
    }
}