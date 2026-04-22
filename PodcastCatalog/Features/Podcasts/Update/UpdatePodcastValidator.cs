using FluentValidation;

namespace PodcastCatalog.Features.Podcasts.Update
{
    public class UpdatePodcastValidator : AbstractValidator<UpdatePodcastCommand>
    {
        public UpdatePodcastValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
        }
    }
}
