namespace Gateway.Middleware
{
    public interface ILoggingService
    {
        Task LogAsync(LogEntity log);
    }
}
