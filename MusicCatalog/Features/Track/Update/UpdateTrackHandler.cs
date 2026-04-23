using MediatR;
using MusicCatalog.Features.Track.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Features.Track.Update
{
    public class updateTrackHandler : IRequestHandler<UpdateTrackCommand, Guid>
    {
        private readonly ITrackRepository _repository;

        public UpdateTrackHandler(ITrackRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {

           
        //    await _repository.AddAsync(track);

        //    return track.Id;
        }
    }
}
