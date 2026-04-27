using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Features.Track
{
    public class TrackContributionRowDTO
    {
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = "";
        public List<TrackContributionRoleDTO> Roles { get; set; } = new();
    }

}
