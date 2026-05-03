namespace FrontendThrowAway.Components.Pages.MusicCatalog.Contributors
{

    public class ContributorModel
    {
        public string? DisplayName { get; set; }
        public string? LegalName { get; set; }
        public string? IPI { get; set; }
        public string? ISNI { get; set; }
    }

    public class ContributorDto
    {
        public Guid Id { get; set; }
        public string? DisplayName { get; set; }
        public string? LegalName { get; set; }
        public string? IPI { get; set; }
        public string? ISNI { get; set; }
    }
}
