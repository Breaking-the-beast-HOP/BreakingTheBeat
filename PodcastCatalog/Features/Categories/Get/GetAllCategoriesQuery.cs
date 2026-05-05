using MediatR;

namespace PodcastCatalog.Features.Categories.Get
{
    public record GetAllCategoriesQuery : IRequest<List<CategoryDTO>>
    {
    }
}
