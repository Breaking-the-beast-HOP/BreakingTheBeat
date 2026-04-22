using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PodcastCatalog.Features.Podcast.Create
{
    public record CreatePodcastCommand(string Title, string Description) : IRequest<List<PodcastDTO>>;

}
