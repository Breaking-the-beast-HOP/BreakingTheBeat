using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Contributors;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class ContributorRepository : IContributorRepository
    {
        private readonly PodcastCatalogDbContext _dbContext;

        public ContributorRepository(PodcastCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contributor> AddAsync(Contributor entity)
        {
            await _dbContext.Contributors.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var contributor = await _dbContext.Contributors
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contributor is null)
                return false;

            _dbContext.Contributors.Remove(contributor);
            await _dbContext.SaveChangesAsync();

            return true;
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

        public async Task<bool> UpdateAsync(Contributor entity)
        {
            _dbContext.Contributors.Update(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}