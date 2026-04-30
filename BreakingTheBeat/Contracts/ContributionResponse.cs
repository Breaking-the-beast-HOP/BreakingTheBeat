namespace Gateway.Contracts
{
    public class ContributionResponse
    {
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = "";
        public List<ContributionRoleResponse> Roles { get; set; } = new();
    }

}
