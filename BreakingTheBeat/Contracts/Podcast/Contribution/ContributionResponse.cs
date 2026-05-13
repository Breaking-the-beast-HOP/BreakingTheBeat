namespace Gateway.Contracts.Podcast.Contribution
{
    public class ContributionResponse
    {
        public Guid Id { get; set; }
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = null!;
        public Guid ContributorRoleId { get; set; }
        public string ContributorRoleName { get; set; } = null!;
    }
}
