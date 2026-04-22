using SharedKernel.Interfaces.Base;
using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Features.Podcasts
{
    public interface IPodcastRepository : ICrudRepository<Podcast>
    {
    }
}
