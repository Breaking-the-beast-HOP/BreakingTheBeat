using MediatR;

namespace Gateway.Middleware
{
    public class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

        public ExceptionHandlingBehavior(
            ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
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

                var exception = new LogEntity()
                {
                    Timestamp = DateTime.UtcNow,
                    LogLevel = LogLevel.Error,
                    Message = ex.Message,
                    RequestName = requestName
                };

                _logger.LogError(
                    "Timestamp: {Timestamp}, Level: {Level}, Message: {Message}, Request: {Request}",
                    exception.Timestamp,
                    exception.LogLevel,
                    exception.Message,
                    exception.RequestName);

                throw;
            }
        }
    }
}