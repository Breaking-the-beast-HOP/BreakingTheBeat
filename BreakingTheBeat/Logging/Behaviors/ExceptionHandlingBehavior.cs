using MediatR;
using ILogger = Serilog.ILogger;

namespace Gateway.Logging.Behaviors
{
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExceptionHandlingBehavior(
            ILogger logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger.ForContext<ExceptionHandlingBehavior<TRequest, TResponse>>();
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
                var httpContext = _httpContextAccessor.HttpContext;

                var requestName = typeof(TRequest).Name;

                var requestId = httpContext?.Items["requestId"]?.ToString() ?? "UNKNOWN";

                _logger
                    .ForContext("RequestId", requestId)
                    .ForContext("RequestName", requestName)
                    .Error(ex, "Unhandled MediatR exception");

                throw;
            }
        }
    }
}