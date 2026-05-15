namespace Gateway.Contracts.MusicQC;

public record ReleaseQcQueueItemResponse(
    Guid Id,
    Guid ReleaseId,
    string Title,
    string? Version,
    string? EAN,
    DateTime ReceivedAtUtc
);