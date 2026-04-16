using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MusicCatalog.Features.Language.GetLanguages
{
    public record GetLanguagesQuery : IRequest<List<LanguageDTO>>;
}
