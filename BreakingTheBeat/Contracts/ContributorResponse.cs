namespace Gateway.Contracts
{
    public class ContributorResponse
    {
        public Guid Id { get; set; }

        public string LegalName { get; set; }
        public string? DisplayName { get; set; }

        public string? IPI { get; set; }
        public string? ISNI { get; set; }
    }
}