using MediatR;
using MusicCatalogContext.Features.Language.Interfaces;

namespace MusicCatalogContext.Features.Language.GetLanguages
{
    public class GetLanguagesHandler : IRequestHandler<GetLanguagesQuery, List<LanguageDTO>>
    {
        private readonly ILanguageRepository _repository;

        public GetLanguagesHandler(ILanguageRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LanguageDTO>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
        {
            var languages = await _repository.GetAllAsync();

            return languages.Select(l => new LanguageDTO
            {
                Id = l.Id,
                Name = l.Name,
                ISO = l.ISO
            }).ToList();
        }
    }
}