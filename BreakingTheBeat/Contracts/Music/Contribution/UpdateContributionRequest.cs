namespace Gateway.Contracts.Music.Contribution
{
    public class UpdateContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}
