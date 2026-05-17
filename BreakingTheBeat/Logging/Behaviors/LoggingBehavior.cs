using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using MediatR;
using ILogger = Serilog.ILogger;

namespace Gateway.Logging.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(
            ILogger logger)
        {
            _logger = logger.ForContext<LoggingBehavior<TRequest, TResponse>>();
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {

            var requestName = typeof(TRequest).Name;

            _logger
                .ForContext("RequestName", requestName)
                .Information("Handling {@Request}", SanitizeRequest(request));

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

        private object SanitizeRequest(TRequest request)
        {
            var json = JsonSerializer.Serialize(request);

            json = Regex.Replace(json, "\"Password\":\".*?\"", "\"Password\":\"***REDACTED***\"");

            return JsonSerializer.Deserialize<object>(json)!;
        }
    }
}