namespace MusicQualityControl.Domain.ReadModels;

public class ReleaseQcQueueItem
{
    public Guid Id { get; private set; }

    public Guid ReleaseId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Version { get; private set; }

    public string? EAN { get; private set; }

    public DateTime ReceivedAtUtc { get; private set; }

    private ReleaseQcQueueItem() { }

    public static ReleaseQcQueueItem Create(
        Guid releaseId,
        string title,
        string? version,
        string? ean)
    {
        return new ReleaseQcQueueItem
        {
            Id = Guid.NewGuid(),
            ReleaseId = releaseId,
            Title = title,
            Version = version,
            EAN = ean,
            ReceivedAtUtc = DateTime.UtcNow
        };
    }
}