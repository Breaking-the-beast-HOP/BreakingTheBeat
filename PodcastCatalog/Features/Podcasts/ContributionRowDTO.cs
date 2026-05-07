using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Podcasts
{
    public class ContributionRowDTO
    {
        public Guid Id { get; set; }

        // Contributor details
        public Guid ContributorId { get; private set; }
        public string ContributorName { get; private set; } = null!;

        // Role details
        public Guid ContributorRoleId { get; private set; }
        public string ContributorRoleName { get; private set; } = null!;

        public bool HasRights { get; private set; }
    }
}
