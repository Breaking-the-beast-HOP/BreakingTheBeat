namespace MusicCatalog.Features.Track.Update
{
    public record UpdateContributionCommand(Guid ContributorId, Guid ContributorRoleId);
}