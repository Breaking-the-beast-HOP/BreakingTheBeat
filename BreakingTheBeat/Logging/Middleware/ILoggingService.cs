namespace Gateway.Logging.Middleware
{
    public interface ILoggingService
    {
        Task LogAsync(LogEntity log);
    }
}
