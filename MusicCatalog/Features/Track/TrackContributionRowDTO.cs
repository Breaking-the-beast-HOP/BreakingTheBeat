namespace MusicCatalog.Features.Track
{
    public class TrackContributionRowDTO
    {
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = "";
        public List<TrackContributionRoleDTO> Roles { get; set; } = new();
    }

}
