using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Contributor;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class ContributorRepository : IContributorRepository
    {
        private readonly MusicCatalogDbContext _context;

        public ContributorRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Contributor> AddAsync(Contributor entity)
        {
            _context.Contributors.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contributor>> GetAllAsync()
        {
            return await _context.Contributors.ToListAsync();
        }

        public async Task<Contributor?> GetByIdAsync(Guid id)
        {
            return await _context.Contributors
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Contribution>> GetContributionsByContributorIdAsync(Guid contributorId)
        {
            return await _context.Contributions
                .AsNoTracking()
                .Include(c => c.Role)
                .Include(c => c.Track)
                .Include(c => c.Release)
                .Where(c => c.ContributorId == contributorId)
                .ToListAsync();
        }

        public Task<bool> UpdateAsync(Contributor entity)
        {
            throw new NotImplementedException();
        }
    }
}
