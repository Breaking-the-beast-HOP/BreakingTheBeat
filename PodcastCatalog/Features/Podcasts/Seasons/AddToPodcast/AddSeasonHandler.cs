using FluentValidation;
using MediatR;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Seasons.AddToPodcast;

public class AddSeasonHandler : IRequestHandler<AddSeasonCommand, SeasonDTO>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IValidator<AddSeasonCommand> _validator;

    public AddSeasonHandler(
        IPodcastRepository podcastRepository,
        IValidator<AddSeasonCommand> validator)
    {
        _podcastRepository = podcastRepository;
        _validator = validator;
    }

    public async Task<SeasonDTO> Handle(AddSeasonCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var season = await _podcastRepository.AddSeasonAsync(
            request.PodcastId,
            request.Title,
            request.SeasonNumber);

        return new SeasonDTO
        {
            Id = season.Id,
            Title = season.Title,
            SeasonNumber = season.SeasonNumber,
            ReleaseDate = null,
            Episodes = new()
        };
    }
}