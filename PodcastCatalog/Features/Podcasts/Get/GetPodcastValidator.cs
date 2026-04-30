using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
