using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Languages.UpdateLanguage
{
    public record UpdateLanguageCommand(Guid Id, string Name, string ISO) : IRequest<LanguageDTO>
    {
    }
}
