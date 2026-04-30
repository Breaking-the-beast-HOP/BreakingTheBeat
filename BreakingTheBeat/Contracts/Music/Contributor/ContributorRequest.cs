namespace Gateway.Contracts.Music.Contributor
{
    public class ContributorRequest
    {
        public string? DisplayName { get; set; }
        public string? LegalName { get; set; }
        public string? IPI { get; set; }
        public string? ISNI { get; set; }
    }
}
