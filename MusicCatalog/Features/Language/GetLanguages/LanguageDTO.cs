using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Features.Language.GetLanguages
{
    public class LanguageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ISO { get; set; } = string.Empty;
    }

}
