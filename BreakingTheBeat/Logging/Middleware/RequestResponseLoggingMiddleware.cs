using System.Text;
using System.Threading.Tasks;
using Gateway.Logging.Entities;
using Gateway.Logging.Services;

namespace Gateway.Logging.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["requestId"] = requestId;

            var loggingService = context.RequestServices.GetRequiredService<ILoggingService>();

            LogRequest(context, requestId, loggingService);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                LogResponse(context, requestId, loggingService);

                responseBody.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);

                context.Response.Body = originalBodyStream;
            }
        }

        private async Task LogRequest(HttpContext context, string requestId, ILoggingService loggingService)
        {
            var log = new LogEntity()
            {
                RequestId = requestId,
                LogLevel = ApplicationLogLevel.Info,
                Message = "Incoming Request",
                Path = context.Request.Path,
                Method = context.Request.Method
            };
            await loggingService.LogAsync(log);
            _logger.LogInformation(
                $"RequestId: {log.RequestId}, " +
                $"Timestamp: {log.Timestamp}, " +
                $"Type: {log.LogLevel}, " +
                $"Message: {log.Message}, " +
                $"Path: {log.Path}, " +
                $"Method: {log.Method}"
                );
        }

        private async Task LogResponse(HttpContext context, string requestId, ILoggingService loggingService)
        {
            var log = new LogEntity()
            {
                RequestId = requestId,
                LogLevel = ApplicationLogLevel.Info,
                Message = $"Outgoing Response - Status {context.Response.StatusCode}.",
                Path = context.Request.Path,
                Method = context.Request.Method
            };
            await loggingService.LogAsync(log);
            _logger.LogInformation(
                $"RequestId: {log.RequestId}, " +
                $"Timestamp: {log.Timestamp}, " +
                $"Type: {log.LogLevel}, " +
                $"Message: {log.Message}, " +
                $"Path: {log.Path}, " +
                $"Method: {log.Method}"
                );
        }
    }
}
