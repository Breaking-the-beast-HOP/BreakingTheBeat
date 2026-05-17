using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class ContributorRepository : ICrudRepository<Contributor>
    {
        private readonly PodcastCatalogDbContext _dbContext;
        public ContributorRepository(PodcastCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Contributor> AddAsync(Contributor entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contributor>> GetAllAsync()
        {
            return await _dbContext.Contributors
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Contributor?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Contributors
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<bool> UpdateAsync(Contributor entity)
        {
            throw new NotImplementedException();
        }
    }
}
