using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.IdentityModel.Protocols;

namespace Gateway.Middleware;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;


        var requestData = JsonSerializer.Serialize(request);
        if (requestName.Contains("Login"))
        {
            requestData = "Sensitive data omitted";
        }

        _logger.LogInformation(
            "Timestamp: {Timestamp}, Level: {Level}, Message: {Message}, RequestData: {RequestData}",
            DateTime.UtcNow,
            "Information",
            $"Handling {requestName}",
            requestData);

        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        _logger.LogInformation(
            "Timestamp: {Timestamp}, Level: {Level}, Message: {Message}",
            DateTime.UtcNow,
            "Information",
            $"Handled {requestName} in {stopwatch.ElapsedMilliseconds}ms");

        return response;
    }
}