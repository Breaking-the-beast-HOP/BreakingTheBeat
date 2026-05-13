using FluentValidation;

namespace PodcastCatalog.Features.Podcasts.Seasons.AddToPodcast
{
    public class AddSeasonValidator : AbstractValidator<AddSeasonCommand>
    {
        public AddSeasonValidator()
        {
            RuleFor(x => x.PodcastId)
                .NotEmpty();

            RuleFor(x => x.SeasonNumber)
                .GreaterThan(0);
        }
    }
}
