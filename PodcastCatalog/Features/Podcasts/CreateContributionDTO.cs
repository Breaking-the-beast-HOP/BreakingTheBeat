namespace PodcastCatalog.Features.Podcasts
{
    public class CreateContributionDTO
    {
        public Guid ContributorId { get; set; }
        public Guid ContributorRoleId { get; set; }
        public bool HasRights { get; set; }
    }
}
