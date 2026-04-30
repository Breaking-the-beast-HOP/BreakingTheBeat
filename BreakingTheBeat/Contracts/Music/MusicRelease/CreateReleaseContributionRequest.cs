namespace Gateway.Contracts.Music.Release
{
    public class CreateReleaseContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}