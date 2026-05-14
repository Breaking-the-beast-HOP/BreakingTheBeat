using Gateway.Logging.Entities;
using Gateway.Logging.Services;
using MediatR;

namespace Gateway.Logging.Behaviors
{
    public class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggingService _loggingService;

        public ExceptionHandlingBehavior(
            ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger,
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
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                var requestId = _httpContextAccessor
                    .HttpContext?
                    .Items["requestId"]?
                    .ToString();

                var log = new LogEntity()
                {
                    RequestId = requestId ?? "UNKNOWN",
                    LogLevel = ApplicationLogLevel.Error,
                    RequestName = requestName,
                    Exception = ex.ToString(),
                    Message = $"Error in {requestName}: {ex.Message}"
                };

                await _loggingService.LogAsync(log);
                _logger.LogError(
                    $"RequestId: {log.RequestId}, " +
                    $"Timestamp: {log.Timestamp}, " +
                    $"Level: {log.LogLevel}, " +
                    $"Message: {log.Message}, " +
                    $"Request: {log.RequestName}, " +
                    $"Exception: {log.Exception}"
                    );

                throw;
            }
        }
    }
}