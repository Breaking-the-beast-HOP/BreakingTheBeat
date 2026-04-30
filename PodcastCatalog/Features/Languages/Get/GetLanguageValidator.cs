using FluentValidation;

namespace PodcastCatalog.Features.Languages.Get
{
    public class GetLanguageValidator : AbstractValidator<GetLanguageQuery>
    {
        public GetLanguageValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Language ID is required.");
        }
    }
}
