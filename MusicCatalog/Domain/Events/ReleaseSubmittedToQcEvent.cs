using MediatR;

namespace MusicCatalog.Domain.Events
{
    public record ReleaseSubmittedToQcEvent(Guid ReleaseId, string Title) : INotification;
}
