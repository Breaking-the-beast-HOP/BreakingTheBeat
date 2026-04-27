using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Track;
using SharedKernel.Interfaces.Base;
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

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == Id);

            if (track is null)
                return false;

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Track>> GetAllAsync()
        {
            return await _context.Tracks
                .Include(t => t.Genre)
                .Include(t => t.MetadataLanguage)
                .Include(t => t.AudioLanguage)
                .ToListAsync();
        }

        public async Task<Track?> GetByIdAsync(Guid id)
        {
            return await _context.Tracks
                .Include(t => t.Genre)
                .Include(t => t.MetadataLanguage)
                .Include(t => t.AudioLanguage)
                .Include(t => t.Contributions)
                    .ThenInclude(c => c.Contributor)
                .Include(t => t.Contributions)
                    .ThenInclude(c => c.Role)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> UpdateAsync(Track entity)
        {
            var existingTrack = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == entity.Id);

            if (existingTrack is null)
                return false;

            _context.Entry(existingTrack).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
