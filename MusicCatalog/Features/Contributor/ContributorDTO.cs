namespace MusicCatalogContext.Features.Contributor
{
    public class ContributorDTO
    {
        public Guid Id { get; set; }

        public string LegalName { get; set; }
        public string? DisplayName { get; set; }

        public string? IPI { get; set; }
        public string? ISNI { get; set; }
    }

}