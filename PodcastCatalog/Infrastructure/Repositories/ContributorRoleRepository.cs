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
            Console.WriteLine($"Connection String: {_dbContext.Database.GetConnectionString()}");
            Console.WriteLine($"Database Name: {_dbContext.Database.GetDbConnection().Database}");

            var roles = await _dbContext.ContributorRoles.AsNoTracking().ToListAsync();
            Console.WriteLine($"Number of roles: {roles.Count}");
            return roles;
        }

        public async Task<ContributorRole?> GetByIdAsync(Guid id)
        {
            return await _dbContext.ContributorRoles
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
