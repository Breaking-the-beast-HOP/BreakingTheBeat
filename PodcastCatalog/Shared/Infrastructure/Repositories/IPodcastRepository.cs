using PodcastCatalog.Shared.Models;

namespace PodcastCatalog.Shared.Infrastructure.Repositories
{
    public interface IPodcastRepository
    {
        Task AddAsync(Podcast podcast, CancellationToken ct);
        Task<Podcast?> GetByIdAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(Podcast podcast, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
