using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class LanguageRepository : ICrudRepository<Language>
    {
        private readonly PodcastCatalogDbContext DbContext;
        public LanguageRepository(PodcastCatalogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task<Language> AddAsync(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
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

        public Task<bool> UpdateAsync(Language entity)
        {
            throw new NotImplementedException();
        }
    }
}
