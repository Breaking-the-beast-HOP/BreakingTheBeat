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
        var userStore = Substitute.For<IUserStore<ApplicationUser>>();
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            userStore, null!, null!, null!, null!, null!, null!, null!, null!);

        var roleStore = Substitute.For<IRoleStore<IdentityRole<Guid>>>();
        _roleManager = Substitute.For<RoleManager<IdentityRole<Guid>>>(
            roleStore, null!, null!, null!, null!);

        var options = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        _dbContext = new UserManagementDbContext(options);

        _handler = new RegisterUserHandler(_userManager, _roleManager, _dbContext);
    }

    private static RegisterUserCommand Cmd(string role = "Admin") =>
        new("user@example.com", "user", "Pass1word!", role);

    [Fact]
    public async Task Handle_UserCreationFails_ReturnsTranslatedErrors()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(
                new IdentityError { Code = "PasswordTooShort", Description = "raw" },
                new IdentityError { Code = "DuplicateEmail", Description = "raw" }));

        var result = await _handler.Handle(Cmd(), CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains("Adgangskoden er for kort.", result.Errors);
        Assert.Contains("Der findes allerede en bruger med denne email.", result.Errors);
    }

    [Fact]
    public async Task Handle_UnknownErrorCode_IsCollapsedToGenericMessage()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(
                new IdentityError { Code = "SomeUnknownCode", Description = "raw" }));

        var result = await _handler.Handle(Cmd(), CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal("Brugeren kunne ikke oprettes.", Assert.Single(result.Errors));
    }

    [Fact]
    public async Task Handle_RoleDoesNotExist_ReturnsRoleNotFound()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);
        _roleManager.RoleExistsAsync("Admin").Returns(false);

        var result = await _handler.Handle(Cmd("Admin"), CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal("Rollen findes ikke.", Assert.Single(result.Errors));
    }

    [Fact]
    public async Task Handle_AddToRoleFails_ReturnsErrors()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);
        _roleManager.RoleExistsAsync(Arg.Any<string>()).Returns(true);
        _userManager.AddToRoleAsync(Arg.Any<ApplicationUser>(), "Admin")
            .Returns(IdentityResult.Failed(
                new IdentityError { Code = "InvalidEmail", Description = "raw" }));

        var result = await _handler.Handle(Cmd("Admin"), CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal("Email-adressen er ikke gyldig.", Assert.Single(result.Errors));
    }

    [Fact]
    public async Task Handle_HappyPath_ReturnsSucceededWithUserId()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);
        _roleManager.RoleExistsAsync("Admin").Returns(true);
        _userManager.AddToRoleAsync(Arg.Any<ApplicationUser>(), "Admin")
            .Returns(IdentityResult.Success);

        var result = await _handler.Handle(Cmd("Admin"), CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.NotNull(result.UserId);
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Handle_HappyPath_AssignsRequestedRole()
    {
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);
        _roleManager.RoleExistsAsync("Content Creator").Returns(true);
        _userManager.AddToRoleAsync(Arg.Any<ApplicationUser>(), "Content Creator")
            .Returns(IdentityResult.Success);

        await _handler.Handle(Cmd("Content Creator"), CancellationToken.None);

        await _userManager.Received(1)
            .AddToRoleAsync(Arg.Any<ApplicationUser>(), "Content Creator");
    }
}
