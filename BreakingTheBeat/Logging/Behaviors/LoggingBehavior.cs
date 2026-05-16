using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using MediatR;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Gateway.Logging.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehavior(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger.ForContext<LoggingBehavior<TRequest, TResponse>>();
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var requestName = typeof(TRequest).Name;

        var requestData = JsonSerializer.Serialize(request);

        if (SensitiveRequests.Any(x => requestName.Contains(x)))
        {
            requestData = "Sensitive data omitted";
        }

        _logger
            .ForContext("RequestName", requestName)
            .Information("Handling {@Request}", request);

        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        _logger
            .ForContext("RequestName", requestName)
            .ForContext("DurationMs", stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture))
            .Information("Handled request");

        return response;
    }

    private static readonly string[] SensitiveRequests = ["Login", "Register", "Token", "Password"];
}