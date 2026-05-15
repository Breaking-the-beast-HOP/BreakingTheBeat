using MusicCatalog.Domain;
using MusicCatalog.Features;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class OutboxRepository : IOutBoxRepository
    {
        private readonly MusicCatalogDbContext _context;

        public OutboxRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<OutboxMessage> AddAsync(OutboxMessage entity)
        {
            await _context.OutboxMessages.AddAsync(entity);

            return entity;
        }
    }
}
