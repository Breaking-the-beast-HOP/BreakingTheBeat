namespace FrontendThrowAway.Models.Tracks.EditTrack
{


    public class ContributionDto
    {
        public Guid ContributorId { get; set; }
        public string ContributorName { get; set; } = "";
        public List<ContributionRoleDto> Roles { get; set; } = new();
    }

    public class ContributionRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
    }
}
