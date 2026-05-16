using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Gateway.Logging.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger logger)
    {
        _next = next;
        _logger = logger.ForContext<ExceptionHandlingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var requestId = context.Items["requestId"]?.ToString() ?? "UNKNOWN";

            _logger.ForContext("RequestId", requestId)
                .ForContext("Path", context.Request.Path)
                .ForContext("StatusCode", StatusCodes.Status500InternalServerError)
                .Error(ex, "Unhandled exception");


            context.Response.ContentType = "application/json";

            var response = new { Message = "An unexpected error occurred." };
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}