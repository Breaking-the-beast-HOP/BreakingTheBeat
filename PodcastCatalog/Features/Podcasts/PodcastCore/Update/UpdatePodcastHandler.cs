using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.Shared;
using SharedKernel;
using SharedKernel.Interfaces.Base;
namespace PodcastCatalog.Features.Podcasts.PodcastCore.Update
{
    public class UpdatePodcastHandler : IRequestHandler<UpdatePodcastCommand, PodcastDTO>
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IValidator<UpdatePodcastCommand> _validator;

        public UpdatePodcastHandler(
            IPodcastRepository podcastRepository,
            IValidator<UpdatePodcastCommand> validator)
        {
            _podcastRepository = podcastRepository;
            _validator = validator;
        }

        public async Task<PodcastDTO> Handle(UpdatePodcastCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var podcast = await _podcastRepository.GetByIdAsync(request.Id);

            if (podcast is null)
                throw new KeyNotFoundException($"Podcast with ID {request.Id} not found.");

            podcast.UpdateDetails(
                request.Title,
                request.Description,
                request.CategoryId,
                request.LanguageId);

            


            var contributions = request.Contributions
                .Where(c =>
                    c.Id != Guid.Empty &&
                    c.ContributorId != Guid.Empty &&
                    c.ContributorRoleId != Guid.Empty)
                .Select(c => (
                    ContributorId: c.ContributorId,
                    ContributorRoleId: c.ContributorRoleId,
                    HasRights: c.HasRights
                ))
                .Distinct()
                .ToList();

            podcast.UpdateContributions(contributions);

            await _podcastRepository.UpdateAsync(podcast);

            return podcast.ToDto();
        }
    }
}