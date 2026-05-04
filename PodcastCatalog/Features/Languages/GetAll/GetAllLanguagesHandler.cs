using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Languages.GetAll
{
    public class GetAllLanguagesHandler : IRequestHandler<GetAllLanguagesQuery, List<LanguageDTO>>
    {
        private readonly IReadRepository<Language> _languageRepository;

        public GetAllLanguagesHandler(IReadRepository<Language> languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<List<LanguageDTO>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
        {
            // Hent alle sprog
            var languages = await _languageRepository.GetAllAsync();
            // Hvis der ikke findes nogen sprog, returner en tom liste
            if (languages == null || !languages.Any())
            {
                return new List<LanguageDTO>
                {
                    new LanguageDTO
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Placeholder",
                        ISO = "xx"
                    }
                };
            }

            var languageDTOs = languages.Select(language => new LanguageDTO
            {
                Id = language.Id,
                Name = language.Name,
                ISO = language.ISO
            }).ToList();
            return languageDTOs;

        }
    }
}
