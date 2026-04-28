using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Features.Track
{
    public class TrackContributionDTO
    {
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = "";
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = "";
    }
}
