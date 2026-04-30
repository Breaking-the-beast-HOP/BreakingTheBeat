using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Languages.Create
{
    public class CreateLanguageValidator : AbstractValidator<CreateNameCommand>
    {
        public CreateLanguageValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.");
        }
    }
}
