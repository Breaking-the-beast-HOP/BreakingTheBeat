using Gateway.Logging.Entities;

namespace Gateway.Logging.Services
{
    public interface ILoggingService
    {
        Task LogAsync(LogEntity log);
    }
}
