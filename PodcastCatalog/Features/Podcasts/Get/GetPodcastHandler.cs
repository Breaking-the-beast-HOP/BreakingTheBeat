using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;

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
                Description = podcast.Description,
                LanguageId = podcast.LanguageId,
                LanguageName = podcast.Language.Name,
                CategoryId = podcast.CategoryId,
                CategoryName = podcast.Category.Name,

                Contributions = podcast.Contributions
                    .Select(h => new ContributionDTO
                    {
                        Id = h.Id,
                        ContributorId = h.ContributorId,
                        ContributorName = h.Contributor.Name,
                        RoleId = h.ContributorRoleId,
                        RoleName = h.Role.Name
                    })
                    .ToList()
            };
        }
    }
}