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
        // Fælles Arrange for alle tests.
        // Vi mocker UserManager og SignInManager, så handleren kan testes
        // uden rigtig database, Identity setup eller HTTP request.
        var userStore = Substitute.For<IUserStore<ApplicationUser>>();

        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            userStore,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var claimsFactory = Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>();

        _signInManager = Substitute.For<SignInManager<ApplicationUser>>(
            _userManager,
            contextAccessor,
            claimsFactory,
            null!,
            null!,
            null!,
            null!);

        // JwtService er ikke mocked her.
        // Vi giver den test-konfiguration, så testen kan kontrollere,
        // at der faktisk bliver lavet en JWT-token ved succesfuldt login.
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-signing-key-long-enough-for-hmac-sha256-please-ignore",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience"
            })
            .Build();

        _jwtService = new JwtService(config);

        _handler = new LoginUserHandler(
            _signInManager,
            _userManager,
            _jwtService);
    }

    // [Fact]
    // Bruges fordi testen dækker ét konkret scenarie:
    // Hvis brugeren ikke findes, skal login fejle med en generisk fejlbesked.
    [Fact]
    public async Task Handle_UserNotFound_ReturnsGenericError()
    {
        // Arrange
        _userManager
            .FindByEmailAsync("missing@example.com")
            .ReturnsNull();

        var command = new LoginUserCommand(
            "missing@example.com",
            "anything");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Token);
        Assert.Equal(new[] { GenericError }, result.Errors);
    }

    [Fact]
    public async Task Handle_UserNotFound_DoesNotCheckPassword()
    {
        // Tester at password-check ikke kaldes,
        // hvis brugeren ikke findes.

        // Arrange
        _userManager
            .FindByEmailAsync(Arg.Any<string>())
            .ReturnsNull();

        var command = new LoginUserCommand(
            "missing@example.com",
            "pw");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _signInManager
            .DidNotReceiveWithAnyArgs()
            .CheckPasswordSignInAsync(default!, default!, default);
    }

    [Fact]
    public async Task Handle_WrongPassword_ReturnsGenericError()
    {
        // Tester at forkert password giver samme generiske fejlbesked.
        // Det er bedre sikkerhed, fordi man ikke afslører om emailen findes.

        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            UserName = "user"
        };

        _userManager
            .FindByEmailAsync(user.Email)
            .Returns(user);

        _signInManager
            .CheckPasswordSignInAsync(user, "wrong", false)
            .Returns(SignInResult.Failed);

        var command = new LoginUserCommand(user.Email, "wrong");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Token);
        Assert.Equal(new[] { GenericError }, result.Errors);
    }

    [Fact]
    public async Task Handle_CorrectPassword_ReturnsTokenAndSucceeded()
    {
        // Tester happy path:
        // Når email og password er korrekte, returneres succes og en JWT-token.

        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            UserName = "user"
        };

        _userManager
            .FindByEmailAsync(user.Email)
            .Returns(user);

        _signInManager
            .CheckPasswordSignInAsync(user, "correct", false)
            .Returns(SignInResult.Success);

        _userManager
            .GetRolesAsync(user)
            .Returns(new List<string> { "Admin" });

        var command = new LoginUserCommand(user.Email, "correct");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Handle_CorrectPassword_PassesRolesToTokenGeneration()
    {
        // Tester at brugerens roller kommer med i JWT-token.
        // Det er vigtigt fordi rollerne senere bruges til authorization.

        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            UserName = "user"
        };

        _userManager
            .FindByEmailAsync(user.Email)
            .Returns(user);

        _signInManager
            .CheckPasswordSignInAsync(user, "correct", false)
            .Returns(SignInResult.Success);

        _userManager
            .GetRolesAsync(user)
            .Returns(new List<string>
            {
                "Admin",
                "Content Creator"
            });

        var command = new LoginUserCommand(user.Email, "correct");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result.Token);

        var roles = token.Claims
            .Where(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToList();

        Assert.Contains("Admin", roles);
        Assert.Contains("Content Creator", roles);
    }

    [Fact]
    public async Task Handle_LockoutOnFailure_IsDisabled()
    {
        // Tester den nuværende implementation:
        // Handleren kalder CheckPasswordSignInAsync med lockoutOnFailure = false.
        //
        // Det betyder at Identitys brute-force beskyttelse ikke aktiveres
        // ved gentagne fejlede loginforsøg.

        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            UserName = "user"
        };

        _userManager
            .FindByEmailAsync(user.Email)
            .Returns(user);

        _signInManager
            .CheckPasswordSignInAsync(
                Arg.Any<ApplicationUser>(),
                Arg.Any<string>(),
                Arg.Any<bool>())
            .Returns(SignInResult.Failed);

        var command = new LoginUserCommand(user.Email, "x");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _signInManager.Received(1)
            .CheckPasswordSignInAsync(user, "x", false);
    }
}