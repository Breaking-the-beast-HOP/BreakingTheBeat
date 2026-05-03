using MusicCatalog.Domain.Entities;
using SharedKernel;

namespace MusicCatalog.Features.Contributor
{
    public interface IContributorRepository : ICrudRepository<Domain.Entities.Contributor>
    {
        Task<List<Contribution>> GetContributionsByContributorIdAsync(Guid contributorId);
    }
}
