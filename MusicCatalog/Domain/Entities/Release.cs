using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Domain.Entities
{
    public class Release
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public Genre Genre { get; set; }
        public Language Language { get; set; }
        public List<Contribution> Contributions { get; set; } = new List<Contribution>();
    }
}
