namespace Gateway.Contracts
{
    public class ContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}
