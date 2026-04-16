using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Domain
{
    public class Contribution
    {
        public Contributor Contributor { get; set; }
        public ContributorRole Role { get; set; }
        public Track? Track { get; set; }
        public Release? Release { get; set; }

        public Contribution(Contributor contributor, ContributorRole role, Track track )
        {
            Contributor = contributor;
            Role = role;
            Track = track;
        }
        public Contribution(Contributor contributor, ContributorRole role, Release release)
        {
            Contributor = contributor;
            Role = role;
            Release = release;
        }
    }
}
