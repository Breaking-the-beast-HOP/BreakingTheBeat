using System.Security.Claims;
using Gateway.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace BreakingTheBeat.Tests.Gateway;

public class PostgresRoleServiceTests
{
    private static PostgresRoleService Build(HttpContext? httpContext)
    {
        // Opretter en fake IHttpContextAccessor med NSubstitute.
        // På den måde kan vi teste servicen uden rigtig ASP.NET runtime.
        var accessor = Substitute.For<IHttpContextAccessor>();

        accessor.HttpContext.Returns(httpContext);

        return new PostgresRoleService(accessor);
    }

    private static HttpContext Authenticated(params string[] roles)
    {
        // Hjælpemetode der opretter en autentificeret bruger
        // med de ønskede roller.
        var ctx = new DefaultHttpContext();

        var claims = roles.Select(role =>
            new Claim(ClaimTypes.Role, role));

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "TestAuth");

        ctx.User = new ClaimsPrincipal(identity);

        return ctx;
    }

    // [Fact]
    // Tester ét konkret scenarie:
    // Hvis der ikke findes en HttpContext, antages requesten at komme fra en background worker.
    [Fact]
    public async Task NoHttpContext_ReturnsBackgroundWorker()
    {
        // Arrange
        var service = Build(httpContext: null);

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_background_worker", role);
    }

    [Fact]
    public async Task WorkerKeyHeaderPresent_ReturnsBackgroundWorker()
    {
        // Tester at requests med Worker-Key får background worker rollen.

        // Arrange
        var ctx = new DefaultHttpContext();

        ctx.Request.Headers["X-Worker-Key"] = "any-non-empty-value";

        var service = Build(ctx);

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_background_worker", role);
    }

    [Fact]
    public async Task WorkerKeyHeaderPresentButEmpty_FallsThroughToAnon()
    {
        // Tester at tom Worker-Key ikke giver adgang som worker.

        // Arrange
        var ctx = new DefaultHttpContext();

        ctx.Request.Headers["X-Worker-Key"] = "";

        var service = Build(ctx);

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_anon", role);
    }

    [Fact]
    public async Task UnauthenticatedUser_ReturnsAnon()
    {
        // Tester at brugere uden login bliver anonyme.

        // Arrange
        var ctx = new DefaultHttpContext();

        ctx.User = new ClaimsPrincipal(new ClaimsIdentity());

        var service = Build(ctx);

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_anon", role);
    }

    [Fact]
    public async Task AdminRole_ReturnsAppAdmin()
    {
        // Tester at Admin rollen bliver mapped korrekt til database-rollen.

        // Arrange
        var service = Build(Authenticated("Admin"));

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_admin", role);
    }

    [Fact]
    public async Task ContentCreatorRole_ReturnsAppContentCreator()
    {
        // Tester mapping mellem applikationsrolle og PostgreSQL rolle.

        // Arrange
        var service = Build(Authenticated("Content creator"));

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_content_creator", role);
    }

    [Fact]
    public async Task QualityControllerRole_ReturnsAppQualityController()
    {
        // Tester mapping for Quality controller rollen.

        // Arrange
        var service = Build(Authenticated("Quality controller"));

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_quality_controller", role);
    }

    [Fact]
    public async Task UnrecognizedRole_FallsThroughToAnon()
    {
        // Tester at ukendte roller ikke giver database-adgang.

        // Arrange
        var service = Build(Authenticated("SomeOtherRole"));

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_anon", role);
    }

    [Fact]
    public async Task AdminTakesPrecedenceOverOtherRoles()
    {
        // Tester at Admin har højeste prioritet,
        // selv hvis brugeren også har andre roller.

        // Arrange
        var service = Build(
            Authenticated("Quality controller", "Admin"));

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_admin", role);
    }

    [Fact]
    public async Task ContentCreatorWithControllerCasing_ReturnsAppContentCreator()
    {
        // Tester at rolle-navnet matcher samme casing som controlleren bruger.

        // Arrange
        var service = Build(Authenticated("Content creator"));

        // Act
        var role = await service.GetDatabaseRole();

        // Assert
        Assert.Equal("app_content_creator", role);
    }
}