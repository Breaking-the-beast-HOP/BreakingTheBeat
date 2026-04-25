using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Domain.Entities
{
    
    public class Contributor
    {
        public Guid Id { get; private set; }

        public string LegalName { get; private set; }
        public string? DisplayName { get; private set; }

        public string? IPI { get; private set; }
        public string? ISNI { get; private set; }

        private Contributor() { } // EF

        public static Contributor Create(string legalName, string? displayName = null, string? ipi = null, string? isni = null)
        {
            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("Legal name is required.");

            var contributor = new Contributor
            {
                Id = Guid.NewGuid(),
                LegalName = legalName,
                DisplayName = string.IsNullOrWhiteSpace(displayName) ? legalName : displayName
            };

            contributor.SetIpi(ipi);
            contributor.SetIsni(isni);

            return contributor;
        }

        // ------------------------
        // Behavior
        // ------------------------

        public void UpdateNames(string legalName, string? displayName)
        {
            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("Legal name is required.");

            LegalName = legalName;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? legalName : displayName;
        }

        public void SetIpi(string? ipi)
        {
            if (!string.IsNullOrWhiteSpace(ipi) && ipi.Length != 11)
                throw new ArgumentException("IPI must be 11 characters.");

            IPI = ipi;
        }

        public void SetIsni(string? isni)
        {
            if (string.IsNullOrWhiteSpace(isni))
            {
                ISNI = null;
                return;
            }
            var normalizedIsni = isni.Replace(" ", "").Trim();

            if (normalizedIsni.Length != 16)
                throw new ArgumentException("ISNI must be 16 characters.");

            ISNI = normalizedIsni;
        }
    }
    
}
