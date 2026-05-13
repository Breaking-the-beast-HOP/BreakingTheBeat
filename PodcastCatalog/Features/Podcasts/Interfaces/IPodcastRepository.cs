using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodcastCatalog.Domain.Entities;
using SharedKernel;

namespace PodcastCatalog.Features.Podcasts.Interfaces
{
    public interface IPodcastRepository: ICrudRepository<Domain.Entities.Podcast>
    {
        Task<Season> AddSeasonAsync(Guid podcastId, string? title, int seasonNumber);
    }
}
