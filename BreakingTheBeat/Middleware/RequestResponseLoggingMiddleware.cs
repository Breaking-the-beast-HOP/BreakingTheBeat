using System.Text;

namespace Gateway.Middleware
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

            LogRequest(context, requestId);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                LogResponse(context, requestId);

                responseBody.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);

                context.Response.Body = originalBodyStream;
            }
        }

        private void LogRequest(HttpContext context, string requestId)
        {
            var log = new LogEntity()
            {
                RequestId = requestId,
                Timestamp = DateTime.UtcNow,
                LogLevel = LogLevel.Info,
                Message = "Incomming Request",
                Path = context.Request.Path,
                RequestName = context.Request.Method
            };
            _logger.LogInformation(
                $"RequestId: {log.RequestId}, " +
                $"Timestamp: {log.Timestamp}, " +
                $"Type: {log.LogLevel}, " +
                $"Message: {log.Message}, " +
                $"Path: {log.Path}, " +
                $"Method: {log.Method}"
                );
        }

        private void LogResponse(HttpContext context, string requestId)
        {
            var log = new LogEntity()
            {
                RequestId = requestId,
                Timestamp = DateTime.UtcNow,
                LogLevel = LogLevel.Info,
                Message = $"Outgoing Response - Status {context.Response.StatusCode}.",
                Path = context.Request.Path,
                RequestName = context.Request.Method
            };
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
