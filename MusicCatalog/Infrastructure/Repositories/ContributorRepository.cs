using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Domain.Entities;
using MusicCatalogContext.Features.Contributor;

namespace MusicCatalogContext.Infrastructure.Repositories
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

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var contributor = await _context.Contributors.FindAsync(Id);

            if (contributor is null)
                return false;

            _context.Contributors.Remove(contributor);
            await _context.SaveChangesAsync();

            return true;
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
