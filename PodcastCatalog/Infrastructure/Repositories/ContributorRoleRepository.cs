using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Infrastructure.Data;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Infrastructure.Repositories
{
    public class ContributorRoleRepository : IReadRepository<ContributorRole>
    {
        private readonly PodcastCatalogDbContext _dbContext;
        public ContributorRoleRepository(PodcastCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ContributorRole>> GetAllAsync()
        {
            return await _dbContext.ContributorRoles
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<ContributorRole?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
