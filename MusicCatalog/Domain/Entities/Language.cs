using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Domain.Entities
{
    public class Language
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ISO { get; private set; }

        private Language() { } // EF Core

        public Language(Guid id, string name, string iso)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Language name cannot be empty");

            if (string.IsNullOrWhiteSpace(iso))
                throw new ArgumentException("ISO code cannot be empty");

            if (iso.Length > 10)
                throw new ArgumentException("ISO code too long");

            Id = id;
            Name = name;
            ISO = iso;
        }
    }
}
