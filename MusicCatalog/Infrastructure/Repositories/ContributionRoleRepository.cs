using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Domain.Entities;
using MusicCatalogContext.Features.ContributorRole;

namespace MusicCatalogContext.Infrastructure.Repositories
{
    public class ContributionRoleRepository : IContributorRoleRepository
    {
        private readonly MusicCatalogDbContext _context;

        public ContributionRoleRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContributorRole>> GetAllAsync()
        {
            return await _context.ContributorRoles.ToListAsync();
        }

        public Task<ContributorRole?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
