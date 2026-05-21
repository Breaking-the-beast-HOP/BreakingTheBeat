using MusicCatalogContext.Domain.Entities;
using SharedKernel;

namespace MusicCatalogContext.Features.Contributor
{
    public interface IContributorRepository : ICrudRepository<Domain.Entities.Contributor>
    {
        Task<List<Contribution>> GetContributionsByContributorIdAsync(Guid contributorId);
    }
}
