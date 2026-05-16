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

        public Task<List<Label>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Label?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Label entity)
        {
            throw new NotImplementedException();
        }
    }
}
