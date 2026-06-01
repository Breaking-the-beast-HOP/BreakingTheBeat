using Microsoft.EntityFrameworkCore;
using MusicQualityControl.Domain.ReadModels;
using MusicQualityControl.Features;

namespace MusicQualityControl.Infrastructure
{
    public class ReleaseQcQueueItemRepository : IReleaseQcQueueItemRepository
    {
        private readonly MusicQualityControlDbContext _context;

        public ReleaseQcQueueItemRepository(MusicQualityControlDbContext context)
        {
            _context = context;
        }

        public async Task<ReleaseQcQueueItem> AddAsync(ReleaseQcQueueItem entity)
        {
            await _context.ReleaseQcQueueItems.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _context.ReleaseQcQueueItems
                .FirstOrDefaultAsync(x => x.ReleaseId == id);

            if (item is null)
                return false;

            _context.ReleaseQcQueueItems.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ReleaseQcQueueItem>> GetAllAsync()
        {
            return await _context.ReleaseQcQueueItems
               .AsNoTracking()
               .OrderBy(x => x.ReceivedAtUtc)
               .ToListAsync();
        }

        public Task<ReleaseQcQueueItem?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
