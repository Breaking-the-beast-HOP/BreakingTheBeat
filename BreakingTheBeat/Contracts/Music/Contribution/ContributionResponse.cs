using Gateway.Contracts.Music.ContributionRole;

namespace Gateway.Contracts.Music.Contribution
{
    public class ContributionResponse
    {
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = "";
        public List<ContributionRoleResponse> Roles { get; set; } = new();
    }

}
