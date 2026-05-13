namespace Gateway.Middleware
{
    public class ILoggingService
    {
        Task LogAsync(LogEntity log);
    }
}
