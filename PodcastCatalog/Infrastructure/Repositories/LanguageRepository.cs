using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Infrastructure.Data;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class LanguageRepository : IReadRepository<Language>
    {
        private readonly PodcastCatalogDbContext DbContext;
        public LanguageRepository(PodcastCatalogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Language>> GetAllAsync()
        {
            return await DbContext.Languages
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Language?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
