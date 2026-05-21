using Microsoft.EntityFrameworkCore;
using MusicCatalog.Features.Label.Create;
using MusicCatalogContext.Domain.Entities;

namespace MusicCatalogContext.Infrastructure.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly MusicCatalogDbContext _context;

        public LabelRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }
        public async Task<Label> AddAsync(Label entity)
        {
            var label = await _context.Labels.AddAsync(entity);
            _context.SaveChanges();
            return label.Entity;

        }

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }


        public async Task<List<Label>> GetAllAsync()
        {
            return await _context.Labels
                .AsNoTracking()
                .Include(x => x.Country)
                .Include(x => x.PrimaryLanguage)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Label?> GetByIdAsync(Guid id)
        {
            return await _context.Labels
                .Include(x => x.Country)
                .Include(x => x.PrimaryLanguage)
                .Include(x => x.ContactPerson.PreferredLanguage)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<bool> UpdateAsync(Label entity)
        {
            throw new NotImplementedException();
        }
    }
}
