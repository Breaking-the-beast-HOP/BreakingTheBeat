using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MusicCatalog.Features.Track.GetById
{
    public record GetTrackByIdQuery(Guid Id) : IRequest<TrackDTO>;
}
