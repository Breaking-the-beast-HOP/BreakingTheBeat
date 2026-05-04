using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Podcasts.Update
{
    public class UpdatePodcastHandler : IRequestHandler<UpdatePodcastCommand, PodcastDTO>
    {
        private readonly ICrudRepository<Podcast> _podcastRepository;
        private readonly IValidator<UpdatePodcastCommand> _validator;

        public UpdatePodcastHandler(
        ICrudRepository<Podcast> podcastRepository,
        IValidator<UpdatePodcastCommand> validator)
        {
            _podcastRepository = podcastRepository;
            _validator = validator;
        }

        public async Task<PodcastDTO> Handle(UpdatePodcastCommand request, CancellationToken cancellationToken)
        {
            // Valider input
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Hent den eksisterende podcast
            var existingPodcast = await _podcastRepository.GetByIdAsync(request.Id);
            if (existingPodcast == null)
            {
                throw new KeyNotFoundException($"Podcast with ID {request.Id} not found.");
            }

            // Opdater podcasten
            existingPodcast.Update(request.Title, request.Description);
            existingPodcast.ChangeLanguage(request.LanguageId);
            await _podcastRepository.UpdateAsync(existingPodcast);

            // Returner den opdaterede podcast
            return new PodcastDTO
            {
                Id = existingPodcast.Id,
                Title = existingPodcast.Title,
                Description = existingPodcast.Description

            };
        }
    }
}
