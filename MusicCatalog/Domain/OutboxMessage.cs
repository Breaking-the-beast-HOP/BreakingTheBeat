namespace MusicCatalogContext.Domain
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public DateTime OccurredOnUtc { get; private set; }
        public string Type { get; private set; } = string.Empty;
        public string Payload { get; private set; } = string.Empty;
        public DateTime? ProcessedOnUtc { get; private set; }
        public string? Error { get; private set; }

        private OutboxMessage() { }

        public static OutboxMessage Create(string type, string payload)
        {
            return new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = type,
                Payload = payload
            };
        }

        public void MarkAsProcessed()
        {
            ProcessedOnUtc = DateTime.UtcNow;
            Error = null;
        }

        public void MarkAsFailed(string error)
        {
            Error = error;
        }
    }
}
