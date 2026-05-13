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
            LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                LogResponse(context);

                responseBody.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);

                context.Response.Body = originalBodyStream;
            }
        }

        private void LogRequest(HttpContext context)
        {
            _logger.LogInformation(
                "Timestamp: {Timestamp}, Type: {Type}, Message: {Message}, Path: {Path}, Method: {Method}",
                DateTime.UtcNow,
                "Info",
                "Incomming Request",
                context.Request.Path,
                context.Request.Method);
        }

        private void LogResponse(HttpContext context)
        {
            _logger.LogInformation(
                "Timestamp: {Timestamp}, Type: {Type}, Message: {Message}, Path: {Path}, Method: {Method}",
                DateTime.UtcNow,
                "INFO",
                $"Outgoing Response - Status {context.Response.StatusCode}",
                context.Request.Path,
                context.Request.Method);
        }
    }
}
