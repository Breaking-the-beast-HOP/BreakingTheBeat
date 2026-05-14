using System.Diagnostics;
using System.Text.Json;
using Gateway.Logging.Entities;
using Gateway.Logging.Services;
using MediatR;

namespace Gateway.Logging.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILoggingService _loggingService;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor,
        ILoggingService loggingService)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _loggingService = loggingService;
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


        if (SensitiveRequests.Any(x => requestName.Contains(x)))
        {
            requestData = "Sensitive data omitted";
        }

        await LogRequest(requestName, requestData, requestId);


        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        await LogResponse(requestName, stopwatch.ElapsedMilliseconds.ToString(), requestId);


        return response;
    }

    private async Task LogRequest(string? requestName, string? requestData, string requestId)
    {
        var log = new LogEntity
        {
            RequestId = requestId,
            LogLevel = ApplicationLogLevel.Info,
            Message = $"Handling {requestName}",
            RequestData = requestData
        };

        await _loggingService.LogAsync(log);
        _logger.LogInformation(
            $"RequestId: {log.RequestId}, " +
            $"Timestamp: {log.Timestamp}, " +
            $"Level: {log.LogLevel}, " +
            $"Message: {log.Message}, " +
            $"RequestData: {log.RequestData}"
            );
    }

    private async Task LogResponse(string requestName, string stopwatch, string requestId)
    {
        var log = new LogEntity
        {
            RequestId = requestId,
            LogLevel = ApplicationLogLevel.Info,
            Message = $"Handled {requestName} in {stopwatch}ms"
        };

        await _loggingService.LogAsync(log);
        _logger.LogInformation(
        $"RequestId: {log.RequestId}, " +
        $"Timestamp: {log.Timestamp}, " +
        $"Level: {log.LogLevel}, " +
        $"Message: {log.Message}"
        );
    }

    private static readonly string[] SensitiveRequests =
        [
            "Login",
            "Register",
            "Token",
            "Password"
        ];


}