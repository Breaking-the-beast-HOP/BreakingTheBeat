// Feature/Podcasts/Get/GetPodcastHandler.cs
using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Podcasts.Get
{
    public class GetPodcastHandler : IRequestHandler<GetPodcastQuery, PodcastDTO>
    {
        private readonly ICrudRepository<Podcast> _podcastRepository;
        private readonly IValidator<GetPodcastQuery> _validator;

        public GetPodcastHandler(ICrudRepository<Podcast> podcastRepository, IValidator<GetPodcastQuery> validator)
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

            return new PodcastDTO
            {
                Id = podcast.Id,
                Title = podcast.Title,
                Description = podcast.Description
            };
        }
    }
}