using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Track;
using System.Threading;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly MusicCatalogDbContext _context;

        public TrackRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Track> AddAsync(Track entity)
        {
            _context.Tracks.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task DeleteAsync(Track entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Track>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Track?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Track entity)
        {
            throw new NotImplementedException();
        }
    }
}
