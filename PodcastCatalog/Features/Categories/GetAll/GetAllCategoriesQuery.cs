using MediatR;

namespace PodcastCatalog.Features.Categories.GetAll
{
    public record GetAllCategoriesQuery : IRequest<List<CategoryDTO>>
    {
    }
}
