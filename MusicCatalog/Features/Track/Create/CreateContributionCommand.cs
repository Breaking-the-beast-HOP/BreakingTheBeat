namespace MusicCatalog.Features.Track.Create
{
    public record CreateContributionCommand(Guid ContributorId,List<Guid> RoleIds);
}