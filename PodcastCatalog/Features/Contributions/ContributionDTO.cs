namespace PodcastCatalog.Features.Contributions
{
    public class ContributionDTO
    {
        public Guid Id { get; set; }
        public Guid ContributorId { get; set; }
        public Guid ContributorRoleId { get; set; }
        public Guid? PodcastId { get; set; }
        public Guid? EpisodeId { get; set; }
        public bool HasRights { get; set; }
    }
}
