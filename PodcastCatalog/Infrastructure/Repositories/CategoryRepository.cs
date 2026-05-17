using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class CategoryRepository : IReadRepository<Category>
    {
        private readonly PodcastCatalogDbContext DbContext;
        public CategoryRepository(PodcastCatalogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await DbContext.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Category?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
