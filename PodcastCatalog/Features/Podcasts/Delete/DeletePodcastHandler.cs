using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Podcasts.Delete
{
    public class DeletePodcastHandler : IRequestHandler<DeletePodcastCommand, Unit>
    {
        private readonly ICrudRepository<Podcast> _podcastRepository;
        private readonly IValidator<DeletePodcastCommand> _validator;
        public DeletePodcastHandler(ICrudRepository<Podcast> podcastRepository, IValidator<DeletePodcastCommand> validator)
        {
            _podcastRepository = podcastRepository;
            _validator = validator;
        }
        public async Task<Unit> Handle(DeletePodcastCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var podcast = await _podcastRepository.GetByIdAsync(request.Id);
            if (podcast == null)
            {
                throw new KeyNotFoundException($"Podcast with ID {request.Id} not found.");
            }
            await _podcastRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
