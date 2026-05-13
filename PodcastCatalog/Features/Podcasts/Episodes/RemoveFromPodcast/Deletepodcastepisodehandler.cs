using MediatR;
using PodcastCatalog.Features.Podcasts.Interfaces;

namespace PodcastCatalog.Features.Podcasts.Episodes.RemoveFromPodcast
{

    public class DeletePodcastEpisodeHandler : IRequestHandler<DeletePodcastEpisodeCommand, bool>
    {
        private readonly IPodcastRepository _podcastRepository;

        public DeletePodcastEpisodeHandler(IPodcastRepository podcastRepository)
        {
            _podcastRepository = podcastRepository;
        }

        public async Task<bool> Handle(DeletePodcastEpisodeCommand request, CancellationToken cancellationToken)
        {
            return await _podcastRepository.DeleteEpisodeAsync(request.EpisodeId);
        }
    }
}