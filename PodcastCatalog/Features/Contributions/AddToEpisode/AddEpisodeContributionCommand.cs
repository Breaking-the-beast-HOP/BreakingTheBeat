using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PodcastCatalog.Features.Contributions.AddToEpisode
{
    public record AddEpisodeContributionCommand(Guid EpisodeId, Guid ContributorId, Guid ContributorRoleId, bool HasRights) : IRequest<ContributionDTO>;
}
