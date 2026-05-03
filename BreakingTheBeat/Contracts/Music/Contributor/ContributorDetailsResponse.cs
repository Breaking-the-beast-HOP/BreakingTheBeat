namespace Gateway.Contracts.Music.Contributor
{
    public class ContributorDetailsResponse
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = "";
        public string? LegalName { get; set; }
        public string? IPI { get; set; }
        public string? ISNI { get; set; }

        public List<ContributorTrackContributionResponse> TrackContributions { get; set; } = new();
        public List<ContributorReleaseContributionResponse> ReleaseContributions { get; set; } = new();
    }

    public class ContributorTrackContributionResponse
    {
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = "";
        public string? ISRC { get; set; }
        public string RoleName { get; set; } = "";
    }

    public class ContributorReleaseContributionResponse
    {
        public Guid ReleaseId { get; set; }
        public string ReleaseTitle { get; set; } = "";
        public string? EAN { get; set; }
        public string RoleName { get; set; } = "";
    }
}
