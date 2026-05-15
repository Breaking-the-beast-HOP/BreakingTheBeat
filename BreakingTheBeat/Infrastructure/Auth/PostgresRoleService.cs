namespace Gateway.Infrastructure.Auth
{

    public class PostgresRoleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostgresRoleService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> GetDatabaseRole()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
            {
                return Task.FromResult("app_background_worker");
            }

            var user = httpContext.User;

            // Check if this is an authenticated worker request
            if (httpContext.Request.Headers.TryGetValue("X-Worker-Key", out var workerKey)
                && !string.IsNullOrEmpty(workerKey))
            {
                // Optionally validate the key here too, or trust that
                // the controller already validated it
                return Task.FromResult("app_background_worker");
            }

            if (user.Identity?.IsAuthenticated != true)
            {
                return Task.FromResult("app_anon");
            }

            if (user.IsInRole("Admin"))
            {
                return Task.FromResult("app_admin");
            }

            if (user.IsInRole("Content Creator"))
            {
                return Task.FromResult("app_content_creator");
            }

            if (user.IsInRole("Quality controller"))
            {
                return Task.FromResult("app_quality_controller");
            }

            return Task.FromResult("app_anon");
        }
    }
}

