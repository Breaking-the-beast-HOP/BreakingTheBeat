namespace Gateway.Contracts.Music.MusicRelease.Create
{
    public class CreateReleaseContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}