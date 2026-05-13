using MediatR;

namespace Gateway.Middleware
{
    public class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExceptionHandlingBehavior(
            ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger,
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
                    RequestId = requestId,
                    Timestamp = DateTime.UtcNow,
                    LogLevel = LogLevel.Error,
                    Message = ex.Message,
                    RequestName = requestName
                };

                _logger.LogError(
                    $"RequestId: {log.RequestId}, " +
                    $"Timestamp: {log.Timestamp}, " +
                    $"Level: {log.LogLevel}, " +
                    $"Message: {log.Message}, " +
                    $"Request: {log.RequestName}"
                    );

                throw;
            }
        }
    }
}