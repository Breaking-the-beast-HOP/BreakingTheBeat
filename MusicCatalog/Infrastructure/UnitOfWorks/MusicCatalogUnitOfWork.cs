using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Interfaces;

namespace MusicCatalogContext.Infrastructure.UnitOfWorks
{
    public class MusicCatalogUnitOfWork : IUnitOfWork
    {
        private readonly MusicCatalogDbContext _context;
        private IDbContextTransaction? _transaction;

        public MusicCatalogUnitOfWork(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_transaction is not null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
            }
        }
    }
}
