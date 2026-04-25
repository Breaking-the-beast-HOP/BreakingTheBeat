using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Contributor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class ContributorRepository : IContributorRepository
    {
        private readonly MusicCatalogDbContext _context;

        public ContributorRepository(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Contributor> AddAsync(Contributor entity)
        {
            _context.Contributors.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contributor>> GetAllAsync()
        {
            return await _context.Contributors.ToListAsync();
        }

        public Task<Contributor?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Contributor entity)
        {
            throw new NotImplementedException();
        }
    }
}
