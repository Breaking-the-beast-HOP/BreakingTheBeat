using FluentValidation;

namespace PodcastCatalog.Features.Podcasts.Get
{
    public class GetPodcastValidator : AbstractValidator<GetPodcastQuery>
    {
        public GetPodcastValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Podcast ID must not be empty.");
        }
    }
}
