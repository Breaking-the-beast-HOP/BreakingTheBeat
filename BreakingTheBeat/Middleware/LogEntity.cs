namespace Gateway.Middleware
{
    public class LogEntity
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? RequestName { get; set; }

        public string? Path { get; set; }

        public string? Method { get; set; }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }
}
