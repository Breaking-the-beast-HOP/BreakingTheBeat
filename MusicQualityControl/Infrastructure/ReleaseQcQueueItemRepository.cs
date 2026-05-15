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
            await  _context.ReleaseQcQueueItems.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
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
