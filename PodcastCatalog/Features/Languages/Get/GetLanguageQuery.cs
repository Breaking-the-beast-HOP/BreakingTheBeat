using MediatR;

namespace PodcastCatalog.Features.Languages.Get
{
    public record GetLanguageQuery(Guid Id) : IRequest<LanguageDTO>;
}
