using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Episodes.AddToPodcast;

public record CreatePodcastEpisodeCommand(
    Guid SeasonId,
    string? Title,
    string? Description,
    TimeSpan? Duration,
    DateTime? AiringDate,
    Guid? TagId
,
    int EpisodeNumber) : IRequest<EpisodeDTO>;
