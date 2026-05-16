using MediatR;

namespace MusicCatalogContext.Features.Language.GetLanguages
{
    public record GetLanguagesQuery : IRequest<List<LanguageDTO>>;
}
