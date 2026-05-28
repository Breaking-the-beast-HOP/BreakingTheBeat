using System.Security.Claims;
using Gateway.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace BreakingTheBeat.Tests.Gateway;

public class PostgresRoleServiceTests
{
    private static PostgresRoleService Build(HttpContext? httpContext)
    {
        var accessor = Substitute.For<IHttpContextAccessor>();
        accessor.HttpContext.Returns(httpContext);
        return new PostgresRoleService(accessor);
    }

    private static HttpContext Authenticated(params string[] roles)
    {
        var ctx = new DefaultHttpContext();
        var claims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        var identity = new ClaimsIdentity(claims, authenticationType: "TestAuth");
        ctx.User = new ClaimsPrincipal(identity);
        return ctx;
    }

    [Fact]
    public async Task NoHttpContext_ReturnsBackgroundWorker()
    {
        var role = await Build(httpContext: null).GetDatabaseRole();

        Assert.Equal("app_background_worker", role);
    }

    [Fact]
    public async Task WorkerKeyHeaderPresent_ReturnsBackgroundWorker()
    {
        var ctx = new DefaultHttpContext();
        ctx.Request.Headers["X-Worker-Key"] = "any-non-empty-value";

        var role = await Build(ctx).GetDatabaseRole();

        Assert.Equal("app_background_worker", role);
    }

    [Fact]
    public async Task WorkerKeyHeaderPresentButEmpty_FallsThroughToAnon()
    {
        var ctx = new DefaultHttpContext();
        ctx.Request.Headers["X-Worker-Key"] = "";

        var role = await Build(ctx).GetDatabaseRole();

        Assert.Equal("app_anon", role);
    }

    [Fact]
    public async Task UnauthenticatedUser_ReturnsAnon()
    {
        var ctx = new DefaultHttpContext();
        ctx.User = new ClaimsPrincipal(new ClaimsIdentity());

        var role = await Build(ctx).GetDatabaseRole();

        Assert.Equal("app_anon", role);
    }

    [Fact]
    public async Task AdminRole_ReturnsAppAdmin()
    {
        var role = await Build(Authenticated("Admin")).GetDatabaseRole();

        Assert.Equal("app_admin", role);
    }

    [Fact]
    public async Task ContentCreatorRole_ReturnsAppContentCreator()
    {
        // PostgresRoleService matches "Content creator" (lowercase c) — see ARCHITECTURE_REVIEW.md #19.
        var role = await Build(Authenticated("Content creator")).GetDatabaseRole();

        Assert.Equal("app_content_creator", role);
    }

    [Fact]
    public async Task QualityControllerRole_ReturnsAppQualityController()
    {
        var role = await Build(Authenticated("Quality controller")).GetDatabaseRole();

        Assert.Equal("app_quality_controller", role);
    }

    [Fact]
    public async Task UnrecognizedRole_FallsThroughToAnon()
    {
        var role = await Build(Authenticated("SomeOtherRole")).GetDatabaseRole();

        Assert.Equal("app_anon", role);
    }

    [Fact]
    public async Task AdminTakesPrecedenceOverOtherRoles()
    {
        var role = await Build(Authenticated("Quality controller", "Admin")).GetDatabaseRole();

        Assert.Equal("app_admin", role);
    }

    [Fact(Skip = "Documents case-sensitivity bug — see ARCHITECTURE_REVIEW.md #19. " +
                 "ReleaseController uses \"Content Creator\" (uppercase C), but PostgresRoleService " +
                 "matches \"Content creator\" (lowercase c). A user authorized by the controller " +
                 "falls through to app_anon at the DB layer. Un-skip after aligning the strings.")]
    public async Task ContentCreatorWithControllerCasing_ShouldMapToAppContentCreator()
    {
        var role = await Build(Authenticated("Content Creator")).GetDatabaseRole();

        Assert.Equal("app_content_creator", role);
    }
}
