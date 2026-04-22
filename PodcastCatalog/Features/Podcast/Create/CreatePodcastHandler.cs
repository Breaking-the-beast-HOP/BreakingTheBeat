using MediatR;
using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Features.Podcast.Create
{
    internal class CreatePodcastHandler : IRequestHandler<CreatePodcastCommand, PodcastDTO>
    {
        private readonly IPodcastRepository _podcastRepository;

        public CreatePodcastHandler(IPodcastRepository podcastRepository)
        {
            _podcastRepository = podcastRepository;
        }

        public async Task<PodcastDTO> Handle(CreatePodcastCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Title))
            {
                throw new ArgumentException("Title is required");
            }

            var podcast = new Podcast(
                Guid.NewGuid(),
                request.Title,
                request.Description);

            await _podcastRepository.AddAsync(podcast, cancellationToken);

            return new PodcastDTO
            {
                Id = podcast.Id,
                Title = podcast.Title,
                Description = podcast.Description
            };
        }

        Task<PodcastDto> IRequestHandler<CreatePodcastCommand, PodcastDto>.Handle(CreatePodcastCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
