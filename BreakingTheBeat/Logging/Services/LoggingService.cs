using Gateway.Logging.Data;
using Gateway.Logging.Entities;

namespace Gateway.Logging.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly LoggingDbContext _context;
        private readonly ILogger<LoggingService> _logger;
        public LoggingService(LoggingDbContext context, ILogger<LoggingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogAsync(LogEntity log)
        {
            _context.GatewayLogs.Add(ToDbEntity(log));
            await _context.SaveChangesAsync();
            try
            {
                _context.GatewayLogs.Add(ToDbEntity(log));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist log entry");
            }
        }
        private GatewayLog ToDbEntity(LogEntity log)
        {
            var dbEntity = new GatewayLog
            {
                RequestId = log.RequestId,
                Timestamp = log.Timestamp,
                Message = log.Message,
                RequestName = log.RequestName,
                Path = log.Path,
                Method = log.Method,
                RequestData = log.RequestData,
                LogLevel = log.LogLevel.ToString(),
                Exception = log.Exception
            };
            return dbEntity;
        }
    }
}
