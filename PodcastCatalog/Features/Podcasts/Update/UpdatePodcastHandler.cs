using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Shared;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Podcasts.Update
{
    public class UpdatePodcastHandler : IRequestHandler<UpdatePodcastCommand, PodcastDTO>
    {
        private readonly ICrudRepository<Podcast> _podcastRepository;
        private readonly ICrudRepository<Contributor> _contributorRepository;
        private readonly IReadRepository<ContributorRole> _roleRepository;
        private readonly IValidator<UpdatePodcastCommand> _validator;

        public UpdatePodcastHandler(
        ICrudRepository<Podcast> podcastRepository,
        ICrudRepository<Contributor> contributorRepository,
        IReadRepository<ContributorRole> roleRepository,
        IValidator<UpdatePodcastCommand> validator)
        {
            _podcastRepository = podcastRepository;
            _contributorRepository = contributorRepository;
            _roleRepository = roleRepository;
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

            foreach (var c in request.Contributors)
            {
                var contributor = await _contributorRepository.GetByIdAsync(c.ContributorId);
                var role = await _roleRepository.GetByIdAsync(c.ContributorRoleId);

                existingPodcast.UpsertContribution(
                    contributor,
                    role,
                    c.HasRights);
            }

            foreach (var contribution in existingPodcast.Contributions.ToList())
            {
                if (!request.Contributors.Any(c => c.ContributorId == contribution.ContributorId))
                {
                    existingPodcast.RemoveContribution(contribution.Id);
                }
            }

            await _podcastRepository.UpdateAsync(existingPodcast);

            // Returner den opdaterede podcast
            return existingPodcast.ToDto();
        }
    }
}
