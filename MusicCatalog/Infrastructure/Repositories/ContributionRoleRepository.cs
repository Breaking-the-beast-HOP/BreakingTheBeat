using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.ContributorRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Infrastructure.Repositories
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
            return await _context.ContributoRoles.ToListAsync();
        }

        public Task<ContributorRole?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
