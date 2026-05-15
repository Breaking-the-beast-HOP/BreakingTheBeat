using FluentValidation;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.Delete
{
    public class DeletePodcastValidator : AbstractValidator<DeletePodcastCommand>
    {
        public DeletePodcastValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Podcast ID is required.");
        }
    }
}
