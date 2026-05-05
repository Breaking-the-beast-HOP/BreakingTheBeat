using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Contributions.AddToPodcast
{
    public class AddPodcastContributionHandler : IRequestHandler<AddPodcastContributionCommand, ContributionDTO>
    {
        private readonly ICrudRepository<Podcast> _podcastRepository;
        private readonly IReadRepository<ContributorRole> _roleRepository;
        private readonly IReadRepository<Contributor> _contributorRepository;
        private readonly IValidator<AddPodcastContributionCommand> _validator;

        public AddPodcastContributionHandler(ICrudRepository<Podcast> podcastRepository, IReadRepository<ContributorRole> roleRepository, IReadRepository<Contributor> contributorRepository, IValidator<AddPodcastContributionCommand> validator)
        {
            _podcastRepository = podcastRepository;
            _roleRepository = roleRepository;
            _contributorRepository = contributorRepository;
            _validator = validator;
        }

        public async Task<ContributionDTO> Handle(AddPodcastContributionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var podcast = await _podcastRepository.GetByIdAsync(request.PodcastId);
            if (podcast == null)
                throw new KeyNotFoundException($"Podcast with ID {request.PodcastId} not found.");

            var role = await _roleRepository.GetByIdAsync(request.ContributorRoleId);
            if (role == null)
                throw new KeyNotFoundException($"Contributor role with ID {request.ContributorRoleId} not found.");

            var contributor = await _contributorRepository.GetByIdAsync(request.ContributorId);
            if (contributor == null)
                throw new KeyNotFoundException($"Contributor with ID {request.ContributorId} not found.");

            var contribution = new Contribution(contributor, role, podcast);
            contribution.SetRights(request.HasRights);

            podcast.AddHost(contribution);

            await _podcastRepository.UpdateAsync(podcast);

            return new ContributionDTO
            {
                Id = contribution.Id,
                ContributorId = contribution.ContributorId,
                ContributorRoleId = contribution.ContributorRoleId,
                PodcastId = contribution.PodcastId,
                EpisodeId = null,
                HasRights = contribution.HasRights
            };

        }
    }
}
