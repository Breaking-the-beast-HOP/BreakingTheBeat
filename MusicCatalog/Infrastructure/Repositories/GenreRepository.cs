using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Genre.Interfaces;
using MusicCatalog.Features.Language.Interfaces;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MusicCatalogDbContext _context;

        public GenreRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }
        public async Task<List<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public Task<Genre?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
