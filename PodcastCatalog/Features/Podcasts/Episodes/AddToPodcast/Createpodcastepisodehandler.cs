using FluentValidation;
using MediatR;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Episodes.AddToPodcast;

public class CreatePodcastEpisodeHandler : IRequestHandler<CreatePodcastEpisodeCommand, EpisodeDTO>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IValidator<CreatePodcastEpisodeCommand> _validator;

    public CreatePodcastEpisodeHandler(IPodcastRepository podcastRepository, IValidator<CreatePodcastEpisodeCommand> validator)
    {
        _podcastRepository = podcastRepository;
        _validator = validator;
    }

    public async Task<EpisodeDTO> Handle(CreatePodcastEpisodeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var episode = await _podcastRepository.AddEpisodeAsync(
            request.SeasonId,
            request.Title,
            request.Description,
            request.Duration,
            request.AiringDate,
            request.TagId);

        return new EpisodeDTO
        {
            Id = episode.Id,
            Title = episode.Title,
            Description = episode.Description,
            EpisodeNumber = episode.EpisodeNumber,
            Duration = episode.Duration,
            AiringDate = episode.AiringDate,
            Status = episode.Status,
            TagId = episode.TagId
        };
    }
}