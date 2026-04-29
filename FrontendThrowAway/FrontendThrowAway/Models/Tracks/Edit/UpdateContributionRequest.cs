namespace FrontendThrowAway.Models.Tracks.EditTrack
{
    public class UpdateContributionRequest
    {
        public Guid ContributorId { get; set; }
        public List<Guid> RoleIds { get; set; } = new();
    }
}
