using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Categories.Get
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDTO>>
    {
        private readonly IReadRepository<Category> _categoryRepository;

        public GetAllCategoriesHandler(IReadRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task<List<CategoryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                return new List<CategoryDTO>
                {
                    new CategoryDTO
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Placeholder",
                        Description = "No categories found."
                    }
                };
            }

            var categoryDTOs = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
            return categoryDTOs;
        }
    }
}
