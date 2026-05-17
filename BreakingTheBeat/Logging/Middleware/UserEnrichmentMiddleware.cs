using System.Security.Claims;
using Serilog.Context;

namespace Gateway.Logging.Middleware
{
    public class UserEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public UserEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.User;

            using (LogContext.PushProperty("UserId",
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous"))
            using (LogContext.PushProperty("Username",
                user.Identity?.Name ?? "anonymous"))
            {
                await _next(context);
            }
        }
    }
}
