using MediatR;
using MusicCatalog.Features.Language.GetLanguages;

namespace MusicCatalog.Features.Languages.GetLanguages
{
    public class GetLanguagesHandler : IRequestHandler<GetLanguagesQuery, List<LanguageDTO>>
    {
        public Task<List<LanguageDTO>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
        {
            var languages = new List<LanguageDTO>
            {
                new LanguageDTO { Id = 1, Name = "Dansk", ISO = "da-DK" },
                new LanguageDTO { Id = 2, Name = "English", ISO = "en-US" },
                new LanguageDTO { Id = 3, Name = "Deutsch", ISO = "de-DE" },
                new LanguageDTO { Id = 4, Name = "Français", ISO = "fr-FR" },
            };

            return Task.FromResult(languages);
        }
    }
}