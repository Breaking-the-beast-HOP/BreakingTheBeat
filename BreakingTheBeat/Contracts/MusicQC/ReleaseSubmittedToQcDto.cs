namespace Gateway.Contracts.MusicQC
{
    public record ReleaseSubmittedToQcDto( Guid ReleaseId,string Title, string? Version,string? EAN);
}
