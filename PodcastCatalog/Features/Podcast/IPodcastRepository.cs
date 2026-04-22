using SharedKernel.Interfaces.Base;
namespace PodcastCatalog.Features.Podcast
{
    internal interface IPodcastRepository : IReadRepository<Domain.Entities.Podcast>
    {
    }
}
