using FluentValidation;

namespace PodcastCatalog.Features.Contributions.AddToPodcast
{
    public class AddPodcastContributionValidation : AbstractValidator<AddPodcastContributionCommand>
    {
        public AddPodcastContributionValidation()
        {
            RuleFor(x => x.PodcastId)
                .NotEmpty().WithMessage("PodcastId is required");
            RuleFor(x => x.ContributorId)
                .NotEmpty().WithMessage("ContributorId is required");
            RuleFor(x => x.ContributorRoleId)
                .NotEmpty().WithMessage("ContributorRoleId is required");
            RuleFor(x => x.HasRights)
                .NotNull().WithMessage("HasRights is required");
        }
    }
}
