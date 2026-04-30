using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class LanguageRepository : ICrudRepository<Language>
    {
        public Task<Language> AddAsync(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid Id)
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

        public Task<bool> UpdateAsync(Language entity)
        {
            throw new NotImplementedException();
        }
    }
}
