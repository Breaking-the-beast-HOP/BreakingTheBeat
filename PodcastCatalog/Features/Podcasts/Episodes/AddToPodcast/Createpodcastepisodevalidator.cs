using FluentValidation;

namespace PodcastCatalog.Features.Podcasts.Episodes.AddToPodcast;

public class CreatePodcastEpisodeValidator : AbstractValidator<CreatePodcastEpisodeCommand>
{
    public CreatePodcastEpisodeValidator()
    {
        RuleFor(x => x.SeasonId)
            .NotEmpty()
            .WithMessage("SeasonId is required.");

        RuleFor(x => x.Title)
            .MaximumLength(250)
            .When(x => x.Title is not null);

        RuleFor(x => x.Duration)
            .Must(d => d >= TimeSpan.Zero)
            .When(x => x.Duration.HasValue)
            .WithMessage("Duration must be zero or positive.");
    }
}
