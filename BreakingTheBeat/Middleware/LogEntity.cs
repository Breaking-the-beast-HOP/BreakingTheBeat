namespace Gateway.Middleware
{
    public class LogEntity
    {
        public string RequestId { get; set; }

        public DateTime Timestamp { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? RequestName { get; set; }

        public string? Path { get; set; }

        public string? Method { get; set; }
        public string? RequestData { get; set; }
    }

    public enum LogLevel
    {
        Startup,
        Debug,
        Info,
        Warning,
        Error,
    }
}
