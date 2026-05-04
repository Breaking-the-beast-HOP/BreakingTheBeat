namespace Gateway.Contracts.Music.Contribution
{
    public class ContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}
