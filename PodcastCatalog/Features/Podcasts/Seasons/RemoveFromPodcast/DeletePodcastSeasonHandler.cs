using MediatR;
using PodcastCatalog.Features.Podcasts.Interfaces;

namespace PodcastCatalog.Features.Podcasts.Seasons.RemoveFromPodcast;

public class DeletePodcastSeasonHandler : IRequestHandler<DeletePodcastSeasonCommand, bool>
{
    private readonly IPodcastRepository _podcastRepository;

    public DeletePodcastSeasonHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<bool> Handle(DeletePodcastSeasonCommand request, CancellationToken cancellationToken)
    {
        return await _podcastRepository.DeleteSeasonAsync(request.SeasonId);
    }
}
