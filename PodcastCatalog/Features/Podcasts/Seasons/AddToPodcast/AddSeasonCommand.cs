using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Seasons.AddToPodcast
{

    public record AddSeasonCommand(Guid PodcastId, string? Title, int SeasonNumber) : IRequest<SeasonDTO>;
}