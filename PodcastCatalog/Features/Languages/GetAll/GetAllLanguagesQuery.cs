using MediatR;

namespace PodcastCatalog.Features.Languages.GetAll
{
    public record GetAllLanguagesQuery : IRequest<List<LanguageDTO>>
    { }
}
