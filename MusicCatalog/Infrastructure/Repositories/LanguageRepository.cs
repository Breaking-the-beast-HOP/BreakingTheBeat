using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Language.Interfaces;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly MusicCatalogDbContext _context;

        public LanguageRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }
        public async Task<List<Language>> GetAllAsync()
        {
            //return await Task.FromResult(new List<Language>
            //{
            //    new Language(Guid.NewGuid(), "Dansk","da-DK"),
            //    new Language (Guid.NewGuid(), "English","en-US" ),
            //    new Language (Guid.NewGuid(), "Deutsch", "de-DE" ),
            //});
            return await _context.Languages.ToListAsync();
        }

        public async Task<Language?> GetByIdAsync(Guid id)
        {
            return await _context.Languages.FindAsync(id);
        }
    }
}
