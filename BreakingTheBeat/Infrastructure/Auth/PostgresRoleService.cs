namespace Gateway.Infrastructure.Auth
{

    public class PostgresRoleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostgresRoleService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetDatabaseRole()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            // Ikke logget ind
            if (user?.Identity?.IsAuthenticated != true)
            {
                return "app_anon";
            }

            // Admin
            if (user.IsInRole("Admin"))
            {
                return "app_admin";
            }

            // Content Creator
            if (user.IsInRole("Content Creator"))
            {
                return "app_content_creator";
            }

            // Quality Controller
            if (user.IsInRole("Quality controller"))
            {
                return "app_quality_controller";
            }

            // Fallback
            return "app_anon";
        }
    }
}
