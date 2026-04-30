using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Release;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class ReleaseRepository : IReleaseRepository
    {

        private readonly MusicCatalogDbContext _context;

        public ReleaseRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Release> AddAsync(Release entity)
        {
            _context.Releases.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Release>> GetAllAsync()
        {
            return await _context.Releases
                   .Include(r => r.Genre)
                   .Include(r => r.Language)
                   .OrderBy(r => r.Title)
                   .AsNoTracking()
                   .ToListAsync();
        }

        public async Task<Release?> GetByIdAsync(Guid id)
        {
            return await _context.Releases
                .Include(r => r.Genre)
                .Include(r => r.Language)

                .Include(r => r.Contributions)
                    .ThenInclude(c => c.Contributor)
                .Include(r => r.Contributions)
                    .ThenInclude(c => c.Role)

                .Include(r => r.Volumes)
                    .ThenInclude(v => v.Tracks)
                        .ThenInclude(vt => vt.Track)

                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<bool> UpdateAsync(Release entity)
        {
            throw new NotImplementedException();
        }
    }
}
