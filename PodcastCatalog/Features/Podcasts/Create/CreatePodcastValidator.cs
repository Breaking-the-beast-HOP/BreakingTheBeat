using FluentValidation;

namespace PodcastCatalog.Features.Podcasts.Create
{
    public class CreatePodcastValidator : AbstractValidator<CreatePodcastCommand>
    {
        public CreatePodcastValidator()
        {
            // Valider Title
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");

            // Valider Description
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.LanguageId)
                .NotEmpty().WithMessage("LanguageId is required");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");

            RuleForEach(x => x.Contributors)
                .ChildRules(contributor =>
                {
                    contributor.RuleFor(c => c.ContributorId)
                        .NotEmpty()
                        .WithMessage("ContributorId is required");

                    contributor.RuleFor(c => c.ContributorRoleId)
                        .NotEmpty()
                        .WithMessage("ContributorRoleId is required");
                });
        }
    }
}
