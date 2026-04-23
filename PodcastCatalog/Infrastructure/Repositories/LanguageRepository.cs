using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class LanguageRepository : ICrudRepository<Language>
    {
        public Task<Language> AddAsync(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Language>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Language?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Language entity)
        {
            throw new NotImplementedException();
        }
    }
}
