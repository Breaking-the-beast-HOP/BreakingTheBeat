using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;
using UserManagement.Domain.Entities;
using UserManagement.Features.User.Register;
using UserManagement.Infrastructure.Persistence;

namespace BreakingTheBeat.Tests.UserManagement;

public class RegisterUserHandlerTests
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManagementDbContext _dbContext;
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        // Fælles Arrange for alle tests.
        // Vi mocker UserManager og RoleManager, så testen ikke afhænger af
        // rigtig Identity database eller eksternt setup.
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

        var roleStore = Substitute.For<IRoleStore<IdentityRole<Guid>>>();

        _roleManager = Substitute.For<RoleManager<IdentityRole<Guid>>>(
            roleStore,
            null!,
            null!,
            null!,
            null!);

        // Her bruger vi EF Core InMemory database.
        // Den gør det muligt at køre testen uden en rigtig PostgreSQL database.
        var options = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new UserManagementDbContext(options);

        _handler = new RegisterUserHandler(
            _userManager,
            _roleManager,
            _dbContext);
    }

    private static RegisterUserCommand Cmd(string role = "Admin") =>
        new(
            "user@example.com",
            "user",
            "Pass1word!",
            role);

    // [Fact]
    // Bruges fordi testen dækker ét konkret scenarie:
    // Hvis brugeroprettelsen fejler, skal Identity-fejl oversættes
    // til brugervenlige danske fejlbeskeder.
    [Fact]
    public async Task Handle_UserCreationFails_ReturnsTranslatedErrors()
    {
        // Arrange
        _userManager
            .CreateAsync(
                Arg.Any<ApplicationUser>(),
                Arg.Any<string>())
            .Returns(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = "raw"
                },
                new IdentityError
                {
                    Code = "DuplicateEmail",
                    Description = "raw"
                }));

        var command = Cmd();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains("Adgangskoden er for kort.", result.Errors);
        Assert.Contains("Der findes allerede en bruger med denne email.", result.Errors);
    }

    [Fact]
    public async Task Handle_UnknownErrorCode_IsCollapsedToGenericMessage()
    {
        // Tester at ukendte Identity error codes ikke sendes direkte videre,
        // men bliver lavet om til en generisk fejlbesked.

        // Arrange
        _userManager
            .CreateAsync(
                Arg.Any<ApplicationUser>(),
                Arg.Any<string>())
            .Returns(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "SomeUnknownCode",
                    Description = "raw"
                }));

        var command = Cmd();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(
            "Brugeren kunne ikke oprettes.",
            Assert.Single(result.Errors));
    }

    [Fact]
    public async Task Handle_RoleDoesNotExist_ReturnsRoleNotFound()
    {
        // Tester at brugeren ikke oprettes færdig,
        // hvis den ønskede rolle ikke findes.

        // Arrange
        _userManager
            .CreateAsync(
                Arg.Any<ApplicationUser>(),
                Arg.Any<string>())
            .Returns(IdentityResult.Success);

        _roleManager
            .RoleExistsAsync("Admin")
            .Returns(false);

        var command = Cmd("Admin");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(
            "Rollen findes ikke.",
            Assert.Single(result.Errors));
    }

    [Fact]
    public async Task Handle_AddToRoleFails_ReturnsErrors()
    {
        // Tester at fejl ved tildeling af rolle bliver håndteret og oversat.

        // Arrange
        _userManager
            .CreateAsync(
                Arg.Any<ApplicationUser>(),
                Arg.Any<string>())
            .Returns(IdentityResult.Success);

        _roleManager
            .RoleExistsAsync(Arg.Any<string>())
            .Returns(true);

        _userManager
            .AddToRoleAsync(Arg.Any<ApplicationUser>(), "Admin")
            .Returns(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "InvalidEmail",
                    Description = "raw"
                }));

        var command = Cmd("Admin");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(
            "Email-adressen er ikke gyldig.",
            Assert.Single(result.Errors));
    }

    [Fact]
    public async Task Handle_HappyPath_ReturnsSucceededWithUserId()
    {
        // Tester happy path:
        // Når brugeroprettelse lykkes, rollen findes,
        // og brugeren kan tildeles rollen, returneres succes og UserId.

        // Arrange
        _userManager
            .CreateAsync(
                Arg.Any<ApplicationUser>(),
                Arg.Any<string>())
            .Returns(IdentityResult.Success);

        _roleManager
            .RoleExistsAsync("Admin")
            .Returns(true);

        _userManager
            .AddToRoleAsync(Arg.Any<ApplicationUser>(), "Admin")
            .Returns(IdentityResult.Success);

        var command = Cmd("Admin");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.UserId);
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Handle_HappyPath_AssignsRequestedRole()
    {
        // Tester at handleren bruger den rolle,
        // som kommer fra commanden, og ikke en hardcoded rolle.

        // Arrange
        _userManager
            .CreateAsync(
                Arg.Any<ApplicationUser>(),
                Arg.Any<string>())
            .Returns(IdentityResult.Success);

        _roleManager
            .RoleExistsAsync("Content Creator")
            .Returns(true);

        _userManager
            .AddToRoleAsync(
                Arg.Any<ApplicationUser>(),
                "Content Creator")
            .Returns(IdentityResult.Success);

        var command = Cmd("Content Creator");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userManager.Received(1)
            .AddToRoleAsync(
                Arg.Any<ApplicationUser>(),
                "Content Creator");
    }
}