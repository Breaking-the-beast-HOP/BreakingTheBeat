using MediatR;

namespace MusicCatalogContext.Domain.Events
{
    public record ReleaseSubmittedToQcEvent(Guid ReleaseId, string Title) : INotification;
}
