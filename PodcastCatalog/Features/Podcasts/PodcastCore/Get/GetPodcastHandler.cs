using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.Shared;
using SharedKernel;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.Get
{
    public class GetPodcastHandler : IRequestHandler<GetPodcastQuery, PodcastDTO>
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IValidator<GetPodcastQuery> _validator;

        public GetPodcastHandler(IPodcastRepository podcastRepository, IValidator<GetPodcastQuery> validator)
        {
            _podcastRepository = podcastRepository;
            _validator = validator;
        }

        public async Task<PodcastDTO> Handle(GetPodcastQuery request, CancellationToken cancellationToken)
        {
            // Valider input
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Hent podcasten
            var podcast = await _podcastRepository.GetByIdAsync(request.Id);
            if (podcast == null)
            {
                throw new KeyNotFoundException($"Podcast with ID {request.Id} not found.");
            }

            return podcast.ToDto();
        }
    }
}