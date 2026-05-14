using Serilog;
using ILogger = Serilog.ILogger;

namespace Gateway.Logging.Middleware;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger logger)
    {
        _next = next;
        _logger = logger.ForContext<RequestResponseLoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();

        context.Items["requestId"] = requestId;

        _logger
            .ForContext("RequestId", requestId)
            .ForContext("Path", context.Request.Path)
            .ForContext("Method", context.Request.Method)
            .Information("Incoming request");

        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();

        context.Response.Body = responseBody;

        await _next(context);

        _logger
            .ForContext("RequestId", requestId)
            .ForContext("Path", context.Request.Path)
            .ForContext("Method", context.Request.Method)
            .ForContext("StatusCode", context.Response.StatusCode)
            .Information("Outgoing response");

        responseBody.Seek(0, SeekOrigin.Begin);

        await responseBody.CopyToAsync(originalBodyStream);

        context.Response.Body = originalBodyStream;
    }

}