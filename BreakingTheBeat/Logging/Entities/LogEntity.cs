namespace Gateway.Logging.Entities
{
    public class LogEntity
    {
        public int Id { get; set; }
        public string RequestId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApplicationLogLevel LogLevel { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? RequestName { get; set; }

        public string? Path { get; set; }

        public string? Method { get; set; }
        public string? RequestData { get; set; }
        public string? Exception { get; set; }
    }

    public enum ApplicationLogLevel
    {
        System,
        Debug,
        Info,
        Warning,
        Error,
    }
}
