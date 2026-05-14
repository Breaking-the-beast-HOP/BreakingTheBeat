using System.Diagnostics;
using System.Text.Json;
using MediatR;

namespace Gateway.Logging.Middleware;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var requestId = _httpContextAccessor
            .HttpContext?
            .Items["requestId"]?
            .ToString();

        var requestData = JsonSerializer.Serialize(request);
        if (requestName.Contains("Login"))
        {
            requestData = "Sensitive data omitted";
        }

        LogRequest(requestName, requestData, requestId);


        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        LogResponse(requestName, stopwatch.Elapsed.ToString(), requestId);


        return response;
    }

    private void LogRequest(string? requestName, string? requestData, string requestId)
    {
        var log = new LogEntity
        {
            RequestId = requestId,
            Timestamp = DateTime.UtcNow,
            LogLevel = LogLevel.Info,
            Message = $"Handling {requestName}",
            RequestData = requestData
        };
        _logger.LogInformation(
            $"RequestId: {log.RequestId}, " +
            $"Timestamp: {log.Timestamp}, " +
            $"Level: {log.LogLevel}, " +
            $"Message: {log.Message}, " +
            $"RequestData: {log.RequestData}"
            );
    }

    private void LogResponse(string requestName, string stopwatch, string requestId)
    {
        var log = new LogEntity
        {
            RequestId = requestId,
            Timestamp = DateTime.UtcNow,
            LogLevel = LogLevel.Info,
            Message = $"Handled {requestName} in {stopwatch}ms"
        };
        _logger.LogInformation(
        $"RequestId: {log.RequestId}, " +
        $"Timestamp: {log.Timestamp}, " +
        $"Level: {log.LogLevel}, " +
        $"Message: {log.Message}"
        );
    }


}