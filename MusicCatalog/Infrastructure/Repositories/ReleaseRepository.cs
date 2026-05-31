using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Domain.Entities;
using MusicCatalogContext.Features.MusicRelease;

namespace MusicCatalogContext.Infrastructure.Repositories
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
            await _context.Releases.AddAsync(entity);

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

        public async Task<bool> UpdateAsync(Release entity)
        {
            var existingRelease = await _context.Releases.FirstOrDefaultAsync(t => t.Id == entity.Id);


            if (existingRelease is null)
                return false;

            _context.Entry(existingRelease).CurrentValues.SetValues(entity);

            return true;
        }
    }
}
