using System.Net.Http.Json;
using System.Text.Json;
using Gateway.Contracts.MusicQC;
using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Infrastructure;

namespace OutboxWorker;

public class MusicCatalogOutboxProcessor
{
    private readonly MusicCatalogDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MusicCatalogOutboxProcessor> _logger;

    public MusicCatalogOutboxProcessor(MusicCatalogDbContext context, IHttpClientFactory httpClientFactory, ILogger<MusicCatalogOutboxProcessor> logger)
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

        var client = _httpClientFactory.CreateClient("QcApi");

        foreach (var message in messages)
        {
            try
            {
                if (message.Type == "ReleaseSubmittedToQc")
                {
                    var dto = JsonSerializer.Deserialize<ReleaseSubmittedToQcDto>(
                        message.Payload,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (dto is null)
                        throw new InvalidOperationException("Outbox payload could not be deserialized.");

                    var response = await client.PostAsJsonAsync(
                        "api/qc/releases",
                        dto,
                        cancellationToken);

                    response.EnsureSuccessStatusCode();
                }
                else if (message.Type == "ReleaseWithdrawnFromQc")
                {
                    var dto = JsonSerializer.Deserialize<ReleaseDeletedDto>(
                        message.Payload,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    if (dto is null)
                        throw new InvalidOperationException("Outbox payload could not be deserialized.");
                    var response = await client.DeleteAsync(
                        $"api/qc/releases/{dto.ReleaseId}",
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