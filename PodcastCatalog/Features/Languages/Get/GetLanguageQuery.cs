using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Languages.Get
{
    public record GetLanguageQuery(Guid Id) : IRequest<LanguageDTO>;
}
