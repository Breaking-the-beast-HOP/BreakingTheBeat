using System.Net;
using System.Text.Json;
using Gateway.Logging.Entities;
using Gateway.Logging.Services;

namespace Gateway.Logging.Middleware
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

                // Resolv ILoggingService fra HttpContext.RequestServices
                var loggingService = context.RequestServices.GetRequiredService<ILoggingService>();

                await LogException(context, ex, loggingService);

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

        private async Task LogException(HttpContext context, Exception ex, ILoggingService loggingService)
        {
            var log = new LogEntity
            {
                RequestId = context.Items["requestId"]?.ToString() ?? "UNKNOWN",
                LogLevel = ApplicationLogLevel.Error,
                Exception = ex.ToString(),
                Path = context.Request.Path,
                Method = context.Request.Method,
                Message = $"An unexpected error occurred with code: {context.Response.StatusCode}"
            };
            await loggingService.LogAsync(log);
            _logger.LogError(
                $"RequestId: {log.RequestId}, " +
                $"Timestamp: {log.Timestamp}, " +
                $"Level: {log.LogLevel}, " +
                $"Exception: {log.Exception}, " +
                $"Path: {log.Path}, " +
                $"Method: {log.Method}"
                );
        }
    }
}
