using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Podcasts.Update
{
    public record UpdatePodcastCommand(Guid Id, string Title, string Description) : IRequest<PodcastDTO>
    {
    }
}
