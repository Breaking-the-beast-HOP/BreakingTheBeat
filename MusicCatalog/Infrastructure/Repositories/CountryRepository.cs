using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Domain.Entities;
using MusicCatalogContext.Features.Countries.Get;

namespace MusicCatalogContext.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MusicCatalogDbContext _context;

        public CountryRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public Task<Country?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
