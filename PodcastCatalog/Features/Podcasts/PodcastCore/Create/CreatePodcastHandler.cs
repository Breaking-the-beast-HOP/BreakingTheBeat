using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.Shared;
using SharedKernel;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.Create;

public class CreatePodcastHandler : IRequestHandler<CreatePodcastCommand, PodcastDTO>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IValidator<CreatePodcastCommand> _validator;

    public CreatePodcastHandler(IPodcastRepository podcastRepository, IValidator<CreatePodcastCommand> validator)
    {
        _podcastRepository = podcastRepository;
        _validator = validator;
    }

    public async Task<PodcastDTO> Handle(CreatePodcastCommand request,CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var podcast = Podcast.Create(
            title: request.Title,
            description: request.Description,
            categoryId: request.CategoryId,
            languageId: request.LanguageId
        );

        foreach (var contributor in request.Contributors ?? [])
        {
            podcast.AddContribution(
                contributorId: contributor.ContributorId,
                roleId: contributor.ContributorRoleId,
                hasRights: contributor.HasRights
            );
        }

        foreach (var seasonDto in request.Seasons ?? [])
        {
            var season = podcast.AddSeason(
                title: seasonDto.Title,
                seasonNumber: seasonDto.SeasonNumber
            );

            foreach (var episodeDto in seasonDto.Episodes ?? [])
            {
                season.AddEpisode(
                    title: episodeDto.Title,
                    description: episodeDto.Description,
                    duration: episodeDto.Duration,
                    airingDate: episodeDto.AiringDate,
                    tagId: episodeDto.TagId
                );
            }
        }

        await _podcastRepository.AddAsync(podcast);

        return podcast.ToDto();
    }
}