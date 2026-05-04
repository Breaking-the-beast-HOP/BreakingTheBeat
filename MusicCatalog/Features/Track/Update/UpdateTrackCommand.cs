using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MusicCatalog.Features.Track.Update
{
    public record UpdateTrackCommand(Guid Id, string? Title, string? Version, string? ISRC, Guid? GenreId, Guid? MetadataLanguageId, Guid? AudioLanguageId) : IRequest<bool>;
}
