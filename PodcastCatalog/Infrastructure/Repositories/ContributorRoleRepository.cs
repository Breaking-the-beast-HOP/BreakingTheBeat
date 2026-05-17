using Microsoft.EntityFrameworkCore;
using PodcastCatalog.Domain.Entities;
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

        public async Task<ContributorRole?> GetByIdAsync(Guid id)
        {
            return await _dbContext.ContributorRoles
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
