using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Features.Contributors
{
    public interface IContributorRepository : ICrudRepository<Contributor>
    {
    }
}