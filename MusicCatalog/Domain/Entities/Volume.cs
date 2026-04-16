using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Domain.Entities
{
    public class Volume
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Track> Tracks { get; set; } = new List<Track>();
    }
}
