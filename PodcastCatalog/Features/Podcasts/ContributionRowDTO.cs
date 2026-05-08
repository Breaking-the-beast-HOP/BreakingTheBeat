namespace PodcastCatalog.Features.Podcasts
{
    public class ContributionRowDTO
    {
        public Guid Id { get; set; }

        // Contributor details
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = null!;

        // Role details
        public Guid ContributorRoleId { get; set; }
        public string ContributorRoleName { get; set; } = null!;

        public bool HasRights { get; set; }
    }
}
