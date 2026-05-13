using System.Net;
using System.Text.Json;

namespace Gateway.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogException(context, ex);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    Message = "An unexpected error occured."
                };

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }

        private void LogException(HttpContext context, Exception ex)
        {
            _logger.LogError(
                "Timestamp: {Timestamp}, Level: {Level}, Message: {Message}, Path: {Path}, Method: {Method}",
                DateTime.UtcNow,
                "Error",
                ex.Message,
                context.Request.Path,
                context.Request.Method);
        }
    }
}
