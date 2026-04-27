using MediatR;

namespace MusicCatalog.Features.Language.GetLanguages
{
    public record GetLanguagesQuery : IRequest<List<LanguageDTO>>;
}
