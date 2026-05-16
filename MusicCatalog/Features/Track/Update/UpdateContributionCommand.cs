namespace MusicCatalogContext.Features.Track.Update
{
    public record UpdateContributionCommand(Guid ContributorId, Guid ContributorRoleId);
}