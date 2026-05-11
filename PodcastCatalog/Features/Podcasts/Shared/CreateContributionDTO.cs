namespace PodcastCatalog.Features.Podcasts.Shared
{
    public class CreateContributionDTO
    {
        public Guid ContributorId { get; set; }
        public Guid ContributorRoleId { get; set; }
        public bool HasRights { get; set; }
    }
}
