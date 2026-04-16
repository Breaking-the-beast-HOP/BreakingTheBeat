using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Domain
{
    public class Language
    {
        public int Id { get; }
        public string Name { get; }
        public string ISO { get; }

        public Language(int id, string name, string iso)
        {
            Id = id;
            Name = name;
            ISO = iso;
        }
    }
}
