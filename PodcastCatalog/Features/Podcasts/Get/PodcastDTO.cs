namespace PodcastCatalog.Features.Podcasts.Get
{
    public class PodcastDTO : SharedDTO
    {
        public List<ContributionDTO> Contributions { get; set; } = new();
    }
    public class ContributionDTO
    {
        public Guid Id { get; set; }
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = null!;
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
