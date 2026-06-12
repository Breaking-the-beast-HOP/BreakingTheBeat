using System.Net.Http.Json;
using System.Text.Json;
using Gateway.Contracts.MusicQC;
using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Infrastructure;

namespace OutboxWorker;

public class MusicQcOutboxProcessor
{
    private readonly MusicCatalogDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MusicCatalogOutboxProcessor> _logger;

    public MusicQcOutboxProcessor(MusicCatalogDbContext context, IHttpClientFactory httpClientFactory, ILogger<MusicCatalogOutboxProcessor> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task ProcessPendingAsync(CancellationToken cancellationToken)
    {
        var messages = await _context.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(20)
            .ToListAsync(cancellationToken);

        if (!messages.Any())
            return;

        var client = _httpClientFactory.CreateClient("CatalogApi");

        foreach (var message in messages)
        {
            try
            {
                if (message.Type == "ReviewStarted")
                {
                    var dto = JsonSerializer.Deserialize<ReviewStartedDto>(
                        message.Payload,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (dto is null)
                        throw new InvalidOperationException("Outbox payload could not be deserialized.");

                    var response = await client.PostAsJsonAsync(
                        "api/catalog/releases/review-started",
                        dto,
                        cancellationToken);

                    response.EnsureSuccessStatusCode();
                }
                else if (message.Type == "ReleaseApproved")
                {
                    var dto = JsonSerializer.Deserialize<ReleaseApprovedDto>(
                        message.Payload,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (dto is null)
                        throw new InvalidOperationException("Outbox payload could not be deserialized.");

                    var response = await client.PostAsJsonAsync(
                        "api/catalog/releases/approved",
                        dto,
                        cancellationToken);

                    response.EnsureSuccessStatusCode();
                }
                else if (message.Type == "ReleaseRejected")
                {
                    var dto = JsonSerializer.Deserialize<ReleaseRejectedDto>(
                        message.Payload,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (dto is null)
                        throw new InvalidOperationException("Outbox payload could not be deserialized.");

                    var response = await client.PostAsJsonAsync(
                        "api/catalog/releases/rejected",
                        dto,
                        cancellationToken);

                    response.EnsureSuccessStatusCode();
                }

                message.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                message.MarkAsFailed(ex.Message);
                _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}