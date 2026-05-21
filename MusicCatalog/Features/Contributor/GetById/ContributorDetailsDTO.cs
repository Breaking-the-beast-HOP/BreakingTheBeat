namespace MusicCatalogContext.Features.Contributor.GetById
{
    public class ContributorDetailsDTO
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = "";
        public string? LegalName { get; set; }
        public string? IPI { get; set; }
        public string? ISNI { get; set; }

        public List<ContributorTrackContributionDTO> TrackContributions { get; set; } = new();
        public List<ContributorReleaseContributionDTO> ReleaseContributions { get; set; } = new();
    }

    public class ContributorTrackContributionDTO
    {
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = "";
        public string? ISRC { get; set; }
        public string RoleName { get; set; } = "";
    }

    public class ContributorReleaseContributionDTO
    {
        public Guid ReleaseId { get; set; }
        public string ReleaseTitle { get; set; } = "";
        public string? EAN { get; set; }
        public string RoleName { get; set; } = "";
    }
}
