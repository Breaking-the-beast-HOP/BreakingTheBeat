using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Podcasts.Create;

public class CreatePodcastHandler : IRequestHandler<CreatePodcastCommand, PodcastDTO>
{
    private readonly ICrudRepository<Podcast> _podcastRepository;
    private readonly ICrudRepository<Contributor> _contributorRepository;
    private readonly IReadRepository<ContributorRole> _roleRepository;
    private readonly IValidator<CreatePodcastCommand> _validator;

    public CreatePodcastHandler(
        ICrudRepository<Podcast> podcastRepository,
        ICrudRepository<Contributor> contributorRepository,
        IReadRepository<ContributorRole> roleRepository,
        IValidator<CreatePodcastCommand> validator)
    {
        _podcastRepository = podcastRepository;
        _contributorRepository = contributorRepository;
        _roleRepository = roleRepository;
        _validator = validator;
    }

    public async Task<PodcastDTO> Handle(CreatePodcastCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var podcast = new Podcast(
            id: Guid.NewGuid(),
            title: request.Title,
            description: request.Description,
            categoryId: request.CategoryId,
            languageId: request.LanguageId
        );

        foreach (var contributorDTO in request.Contributors)
        {
            var contributor = await _contributorRepository.GetByIdAsync(contributorDTO.ContributorId);
            if (contributor == null)
            {
                throw new Exception($"Contributor with ID {contributorDTO.ContributorId} not found.");
            }

            var role = await _roleRepository.GetByIdAsync(contributorDTO.ContributorRoleId);
            if (role == null)
            {
                throw new Exception($"Role with ID {contributorDTO.ContributorRoleId} not found.");
            }

            var contribution = new Contribution(
                id: Guid.NewGuid(),
                contributor,
                role,
                podcast
            );

            contribution.SetRights(contributorDTO.HasRights);
            podcast.AddContribution(contribution);
        }

        await _podcastRepository.AddAsync(podcast);

        var fullPodcast = await _podcastRepository.GetByIdAsync(podcast.Id);

        return new PodcastDTO
        {
            Id = fullPodcast.Id,
            Title = fullPodcast.Title,
            Description = fullPodcast.Description,
            CategoryId = fullPodcast.CategoryId,
            CategoryName = fullPodcast.Category?.Name,
            LanguageId = fullPodcast.LanguageId,
            LanguageName = fullPodcast.Language?.Name,
            Contributions = fullPodcast.Contributions
                .Select(c => new ContributionRowDTO
                {
                    Id = c.Id,
                    ContributorId = c.ContributorId,
                    ContributorName = c.Contributor.Name,
                    ContributorRoleId = c.ContributorRoleId,
                    ContributorRoleName = c.Role.Name,
                    HasRights = c.HasRights
                }).ToList()
        };

    }
}