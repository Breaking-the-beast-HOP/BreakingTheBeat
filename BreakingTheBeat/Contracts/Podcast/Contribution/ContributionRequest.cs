namespace Gateway.Contracts.Podcast.Contribution
{
    public class ContributionRequest
    {
        public Guid ContributorId { get; set; }
        public Guid ContributorRoleId { get; set; }
    }
}
