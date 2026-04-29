using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Languages
{
    public class SharedDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
    }
}
