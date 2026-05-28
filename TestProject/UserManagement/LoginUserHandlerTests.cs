using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using UserManagement.Domain.Entities;
using UserManagement.Features.User.Login;
using UserManagement.Infrastructure.Auth;

namespace BreakingTheBeat.Tests.UserManagement;

public class LoginUserHandlerTests
{
    private const string GenericError = "Forkert email eller adgangskode.";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtService _jwtService;
    private readonly LoginUserHandler _handler;

    public LoginUserHandlerTests()
    {
        var userStore = Substitute.For<IUserStore<ApplicationUser>>();
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            userStore, null!, null!, null!, null!, null!, null!, null!, null!);

        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var claimsFactory = Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _signInManager = Substitute.For<SignInManager<ApplicationUser>>(
            _userManager, contextAccessor, claimsFactory, null!, null!, null!, null!);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-signing-key-long-enough-for-hmac-sha256-please-ignore",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience"
            })
            .Build();
        _jwtService = new JwtService(config);

        _handler = new LoginUserHandler(_signInManager, _userManager, _jwtService);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsGenericError()
    {
        _userManager.FindByEmailAsync("missing@example.com").ReturnsNull();

        var result = await _handler.Handle(
            new LoginUserCommand("missing@example.com", "anything"),
            CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Null(result.Token);
        Assert.Equal(new[] { GenericError }, result.Errors);
    }

    [Fact]
    public async Task Handle_UserNotFound_DoesNotCheckPassword()
    {
        _userManager.FindByEmailAsync(Arg.Any<string>()).ReturnsNull();

        await _handler.Handle(
            new LoginUserCommand("missing@example.com", "pw"),
            CancellationToken.None);

        await _signInManager.DidNotReceiveWithAnyArgs()
            .CheckPasswordSignInAsync(default!, default!, default);
    }

    [Fact]
    public async Task Handle_WrongPassword_ReturnsGenericError()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@example.com", UserName = "user" };
        _userManager.FindByEmailAsync(user.Email).Returns(user);
        _signInManager.CheckPasswordSignInAsync(user, "wrong", false)
            .Returns(SignInResult.Failed);

        var result = await _handler.Handle(
            new LoginUserCommand(user.Email, "wrong"),
            CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Null(result.Token);
        Assert.Equal(new[] { GenericError }, result.Errors);
    }

    [Fact]
    public async Task Handle_CorrectPassword_ReturnsTokenAndSucceeded()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@example.com", UserName = "user" };
        _userManager.FindByEmailAsync(user.Email).Returns(user);
        _signInManager.CheckPasswordSignInAsync(user, "correct", false)
            .Returns(SignInResult.Success);
        _userManager.GetRolesAsync(user).Returns(new List<string> { "Admin" });

        var result = await _handler.Handle(
            new LoginUserCommand(user.Email, "correct"),
            CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Handle_CorrectPassword_PassesRolesToTokenGeneration()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@example.com", UserName = "user" };
        _userManager.FindByEmailAsync(user.Email).Returns(user);
        _signInManager.CheckPasswordSignInAsync(user, "correct", false)
            .Returns(SignInResult.Success);
        _userManager.GetRolesAsync(user).Returns(new List<string> { "Admin", "Content Creator" });

        var result = await _handler.Handle(
            new LoginUserCommand(user.Email, "correct"),
            CancellationToken.None);

        Assert.True(result.Succeeded);
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result.Token);
        var roles = token.Claims
            .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
        Assert.Contains("Admin", roles);
        Assert.Contains("Content Creator", roles);
    }

    [Fact]
    public async Task Handle_LockoutOnFailure_IsDisabled()
    {
        // Documents current behavior: LoginUserHandler passes lockoutOnFailure=false.
        // See ARCHITECTURE_REVIEW.md #7 — this disables Identity's brute-force protection.
        var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@example.com", UserName = "user" };
        _userManager.FindByEmailAsync(user.Email).Returns(user);
        _signInManager.CheckPasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<bool>())
            .Returns(SignInResult.Failed);

        await _handler.Handle(new LoginUserCommand(user.Email, "x"), CancellationToken.None);

        await _signInManager.Received(1)
            .CheckPasswordSignInAsync(user, "x", false);
    }
}
