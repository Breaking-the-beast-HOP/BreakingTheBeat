
namespace Gateway.Middleware
{
    public class LoggingService : ILoggingService
    {
        public Task LogAsync(LogEntity log)
        {
            Console.WriteLine($"RequestId: {log.RequestId}, " +
                $"Timestamp: {log.Timestamp}, " +
                $"LogLevel: {log.LogLevel}, " +
                $"Message: {log.Message}, " +
                $"RequestName: {log.RequestName}, " +
                $"Path: {log.Path}, " +
                $"Method: {log.Method}, " +
                $"RequestData: {log.RequestData}");
            return Task.CompletedTask;
        }
    }
}
